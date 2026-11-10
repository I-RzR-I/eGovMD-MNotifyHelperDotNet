// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-08 07:41
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-10 04:17
// ***********************************************************************
//  <copyright file="CertificateLoader.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using RzR.Extensions.Domain.Primitives;
using RzR.Extensions.Domain.Text;
using MNotifyHelperDotNet.Extensions;

#endregion

#if NETSTANDARD2_1_OR_GREATER || NET || NETCOREAPP3_1_OR_GREATER

using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

#endif


// ReSharper disable IdentifierTypo
// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable JoinDeclarationAndInitializer

namespace MNotifyHelperDotNet.Configurations
{
    /// <summary>
    ///     Certificate loader
    /// </summary>
    /// <remarks></remarks>
    public static class CertificateLoader
    {
        private const string TracePrefix = "MNotifyHelperDotNet: ";
        private const string NoKeyedCertificate = "no-keyed-certificate";
        private const string UnsupportedKeyFormat = "unsupported-key-format";
        private const string KeyMismatch = "key-mismatch";
        private const string MalformedKey = "malformed-key";
        private const string KeyFileMissing = "key-file-missing";
        private const string CertificateFileMissing = "certificate-file-missing";
        private const string IntermediateStoreWriteFailed = "intermediate-store-write-failed";

#if NETSTANDARD2_1_OR_GREATER || NET || NETCOREAPP3_1_OR_GREATER
        private const string RsaOid = "1.2.840.113549.1.1.1";
        private const string EcOid = "1.2.840.10045.2.1";
        private const string Pkcs8KeyLabel = "PRIVATE KEY";
        private const string RsaKeyLabel = "RSA PRIVATE KEY";
        private const string EcKeyLabel = "EC PRIVATE KEY";
        private const string BeginArmour = "-----BEGIN ";
        private const string EndArmour = "-----END ";
        private const string ArmourSuffix = "-----";
        private const string ProcTypeHeader = "Proc-Type:";
        private const string DekInfoHeader = "DEK-Info:";
#endif

        /// <summary>
        ///     Load public certificate
        /// </summary>
        /// <param name="certificatePath">Certificate path</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static X509Certificate2 Public(string certificatePath)
        {
            if (string.IsNullOrWhiteSpace(certificatePath))
                return null;

            var certificate = LoadPublicCertificateFromChain(certificatePath);
            return !certificate.IsNull() 
                ? certificate 
                : File.Exists(certificatePath) 
                    ? new X509Certificate2(certificatePath) 
                    : !Directory.Exists(certificatePath) 
                        ? null 
                        : LoadPublicCertificateFromChain(Path.Combine(certificatePath, "tls.crt"));
        }

        /// <summary>
        ///     Load private certificate
        /// </summary>
        /// <param name="certificatePath">Required. Certificate path</param>
        /// <param name="certificatePassword">Optional. Certificate password. The default value is null.</param>
        /// <returns>The certificate with its private key, or null when the input is not a usable keyed certificate</returns>
        /// <remarks></remarks>
        public static X509Certificate2 Private(string certificatePath, string certificatePassword = null)
        {
            if (string.IsNullOrWhiteSpace(certificatePath))
                return Reject(CertificateFileMissing);

#if NETSTANDARD2_1_OR_GREATER || NET || NETCOREAPP3_1_OR_GREATER
            if (File.Exists(certificatePath) && !string.IsNullOrWhiteSpace(certificatePassword))
                return LoadPrivateFromPfx(certificatePath, certificatePassword);

            if (!Directory.Exists(certificatePath))
                return Reject(CertificateFileMissing);

            return LoadPrivateFromPemDirectory(certificatePath);
#else
            if (!File.Exists(certificatePath))
                return Reject(CertificateFileMissing);

            var result = new X509Certificate2(certificatePath, certificatePassword);
            if (!result.HasPrivateKey)
            {
                result.Reset();

                return Reject(NoKeyedCertificate);
            }

            return result;
#endif
        }

        /// <summary>
        ///     Trace the rejection reason and return null
        /// </summary>
        /// <param name="code">Fixed reason code</param>
        /// <returns>Always null</returns>
        /// <remarks>The message carries only the fixed code, never a path, subject or exception text.</remarks>
        private static X509Certificate2 Reject(string code)
        {
            Trace.TraceWarning(TracePrefix + code);

            return null;
        }

#if NETSTANDARD2_1_OR_GREATER || NET || NETCOREAPP3_1_OR_GREATER

        /// <summary>
        ///     Load the certificate that has a private key from a PFX file
        /// </summary>
        /// <param name="certificatePath">PFX file path</param>
        /// <param name="certificatePassword">PFX password</param>
        /// <returns>The selected certificate, or null when no certificate has a private key</returns>
        /// <remarks></remarks>
        private static X509Certificate2 LoadPrivateFromPfx(string certificatePath, string certificatePassword)
        {
            var certificates = new X509Certificate2Collection();
            certificates.Import(certificatePath, certificatePassword, X509KeyStorageFlags.DefaultKeySet);

            try
            {
                var selected = SelectKeyedCertificate(certificates);
                if (selected == null)
                {
                    DisposeAll(certificates, null);

                    return Reject(NoKeyedCertificate);
                }

                StoreIntermediates(certificates, selected);
                DisposeAll(certificates, selected);

                return selected;
            }
            catch
            {
                DisposeAll(certificates, null);

                throw;
            }
        }

        /// <summary>
        ///     Select the keyed certificate: prefer non-CA, then the highest index in the imported collection order
        /// </summary>
        /// <param name="certificates">Imported certificates</param>
        /// <returns>The selected certificate, or null when none has a private key</returns>
        /// <remarks>The rule depends only on collection order and extensions, never on the current time.</remarks>
        private static X509Certificate2 SelectKeyedCertificate(X509Certificate2Collection certificates)
        {
            X509Certificate2 selected = null;
            var selectedIsCa = false;
            for (var i = 0; i < certificates.Count; i++)
            {
                var candidate = certificates[i];
                if (!candidate.HasPrivateKey)
                    continue;

                var candidateIsCa = IsCertificateAuthority(candidate);
                if (selected == null || !candidateIsCa || selectedIsCa)
                {
                    selected = candidate;
                    selectedIsCa = candidateIsCa;
                }
            }

            return selected;
        }

        /// <summary>
        ///     Check whether the certificate is marked as a certificate authority
        /// </summary>
        /// <param name="certificate">Certificate</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static bool IsCertificateAuthority(X509Certificate2 certificate)
        {
            for (var i = 0; i < certificate.Extensions.Count; i++)
            {
                if (certificate.Extensions[i] is X509BasicConstraintsExtension basicConstraints)
                    return basicConstraints.CertificateAuthority;
            }

            return false;
        }

        /// <summary>
        ///     Dispose the certificates of a collection
        /// </summary>
        /// <param name="certificates">Certificates</param>
        /// <param name="keep">Certificate that must stay alive, or null</param>
        /// <remarks></remarks>
        private static void DisposeAll(X509Certificate2Collection certificates, X509Certificate2 keep)
        {
            for (var i = 0; i < certificates.Count; i++)
            {
                if (!ReferenceEquals(certificates[i], keep))
                    certificates[i].Dispose();
            }
        }

        /// <summary>
        ///     Load the private certificate from a directory that holds tls.crt and tls.key
        /// </summary>
        /// <param name="directory">Directory path</param>
        /// <returns>The certificate with its private key, or null when the input is rejected</returns>
        /// <remarks></remarks>
        private static X509Certificate2 LoadPrivateFromPemDirectory(string directory)
        {
            var certificateFile = Path.Combine(directory, "tls.crt");
            var keyFile = Path.Combine(directory, "tls.key");
            if (!File.Exists(certificateFile))
                return Reject(CertificateFileMissing);

            if (!File.Exists(keyFile))
                return Reject(KeyFileMissing);

            var chain = LoadPublicCertificatesChain(certificateFile);
            try
            {
                if (chain.Count.IsZero())
                    return Reject(CertificateFileMissing);

                if (!TryReadPrivateKeyBlock(File.ReadAllLines(keyFile), out var label, out var base64))
                    return Reject(UnsupportedKeyFormat);

                var result = CombineWithPrivateKey(chain[0], label, base64);
                if (result != null)
                    StoreIntermediates(chain, chain[0]);

                return result;
            }
            finally
            {
                DisposeAll(chain, null);
            }
        }

        /// <summary>
        ///     Read the first PEM block whose label ends with PRIVATE KEY
        /// </summary>
        /// <param name="lines">Key file lines</param>
        /// <param name="label">PEM label of the block</param>
        /// <param name="base64">Base64 body of the block</param>
        /// <returns>True when a complete block was found</returns>
        /// <remarks>Leading blank lines and other blocks (for example EC PARAMETERS) are skipped.</remarks>
        private static bool TryReadPrivateKeyBlock(string[] lines, out string label, out string base64)
        {
            label = null;
            base64 = null;
            for (var i = 0; i < lines.Length; i++)
            {
                if (!TryParseArmour(lines[i], BeginArmour, out var beginLabel) ||
                    !beginLabel.EndsWith(Pkcs8KeyLabel, StringComparison.OrdinalIgnoreCase))
                    continue;

                var body = new StringBuilder();
                for (var j = i + 1; j < lines.Length; j++)
                {
                    if (!TryParseArmour(lines[j], EndArmour, out var endLabel))
                    {
                        if (IsEncryptionHeader(lines[j]))
                            return false;

                        body.Append(lines[j]);
                        continue;
                    }

                    if (!string.Equals(beginLabel, endLabel, StringComparison.OrdinalIgnoreCase))
                        return false;

                    label = beginLabel;
                    base64 = body.ToString();

                    return true;
                }

                return false;
            }

            return false;
        }

        /// <summary>
        ///     Check whether a line is an RFC 1421 encryption header of a PEM block
        /// </summary>
        /// <param name="line">File line</param>
        /// <returns>True for a Proc-Type or DEK-Info header</returns>
        /// <remarks>Encrypted keys are unsupported, so such a block is rejected as an unsupported format.</remarks>
        private static bool IsEncryptionHeader(string line)
        {
            var trimmed = line.Trim();

            return trimmed.StartsWith(ProcTypeHeader, StringComparison.OrdinalIgnoreCase) ||
                   trimmed.StartsWith(DekInfoHeader, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     Parse a PEM armour line
        /// </summary>
        /// <param name="line">File line</param>
        /// <param name="armour">Armour prefix, BEGIN or END</param>
        /// <param name="label">Label between the prefix and the closing dashes</param>
        /// <returns>True when the trimmed line is an armour line of the requested kind</returns>
        /// <remarks>Comparison is case-insensitive.</remarks>
        private static bool TryParseArmour(string line, string armour, out string label)
        {
            label = null;
            var trimmed = line.Trim();
            if (trimmed.Length < armour.Length + ArmourSuffix.Length ||
                !trimmed.StartsWith(armour, StringComparison.OrdinalIgnoreCase) ||
                !trimmed.EndsWith(ArmourSuffix, StringComparison.OrdinalIgnoreCase))
                return false;

            label = trimmed.Substring(armour.Length, trimmed.Length - armour.Length - ArmourSuffix.Length);

            return true;
        }

        /// <summary>
        ///     Combine the certificate with its PEM private key
        /// </summary>
        /// <param name="leaf">Public certificate</param>
        /// <param name="label">PEM label of the key block</param>
        /// <param name="base64">Base64 body of the key block</param>
        /// <returns>A persisted-key certificate, or null when the key is unsupported, malformed or does not match</returns>
        /// <remarks>Key and PFX byte arrays are zeroed before returning.</remarks>
        private static X509Certificate2 CombineWithPrivateKey(X509Certificate2 leaf, string label, string base64)
        {
            var isPkcs8 = string.Equals(label, Pkcs8KeyLabel, StringComparison.OrdinalIgnoreCase);
            var isRsaLabel = string.Equals(label, RsaKeyLabel, StringComparison.OrdinalIgnoreCase);
            var isEcLabel = string.Equals(label, EcKeyLabel, StringComparison.OrdinalIgnoreCase);
            if (!isPkcs8 && !isRsaLabel && !isEcLabel)
                return Reject(UnsupportedKeyFormat);

            var algorithm = leaf.PublicKey.Oid.Value;
            var isRsa = algorithm == RsaOid;
            var isEc = algorithm == EcOid;
            if (!isRsa && !isEc)
                return Reject(UnsupportedKeyFormat);

            if ((isRsaLabel && !isRsa) || (isEcLabel && !isEc))
                return Reject(KeyMismatch);

            byte[] keyBytes = null;
            byte[] pfxBytes = null;
            IDisposable key = null;
            X509Certificate2 withKey = null;
            try
            {
                try
                {
                    keyBytes = Convert.FromBase64String(base64);
                    int bytesRead;
                    if (isRsa)
                    {
                        var rsa = RSA.Create();
                        key = rsa;
                        if (isPkcs8)
                            rsa.ImportPkcs8PrivateKey(keyBytes, out bytesRead);
                        else
                            rsa.ImportRSAPrivateKey(keyBytes, out bytesRead);
                    }
                    else
                    {
                        var ecdsa = ECDsa.Create();
                        key = ecdsa;
                        if (isPkcs8)
                            ecdsa.ImportPkcs8PrivateKey(keyBytes, out bytesRead);
                        else
                            ecdsa.ImportECPrivateKey(keyBytes, out bytesRead);
                    }

                    if (bytesRead != keyBytes.Length)
                        return Reject(MalformedKey);
                }
                catch (Exception e) when (e is FormatException || e is CryptographicException || e is ArgumentException)
                {
                    return Reject(MalformedKey);
                }

                try
                {
                    withKey = isRsa ? leaf.CopyWithPrivateKey((RSA)key) : leaf.CopyWithPrivateKey((ECDsa)key);
                }
                catch (Exception e) when (e is ArgumentException || e is CryptographicException)
                {
                    return Reject(KeyMismatch);
                }

                pfxBytes = withKey.Export(X509ContentType.Pfx);

                return new X509Certificate2(pfxBytes);
            }
            finally
            {
                if (keyBytes != null)
                    CryptographicOperations.ZeroMemory(keyBytes);

                if (pfxBytes != null)
                    CryptographicOperations.ZeroMemory(pfxBytes);

                key?.Dispose();
                withKey?.Dispose();
            }
        }

        /// <summary>
        ///     Write the intermediate CA certificates to the CurrentUser CA store, best effort
        /// </summary>
        /// <param name="certificates">Loaded certificates</param>
        /// <param name="selected">The certificate returned to the caller; it is never written</param>
        /// <remarks>
        ///     Only non-self-signed, key-less CA certificates are written, as public-only copies, and the store is opened only
        ///     when at least one such certificate exists. A store failure is traced and never fails the load.
        /// </remarks>
        private static void StoreIntermediates(X509Certificate2Collection certificates, X509Certificate2 selected)
        {
            X509Certificate2Collection existing = null;
            try
            {
                var candidates = new List<X509Certificate2>();
                for (var i = 0; i < certificates.Count; i++)
                {
                    var certificate = certificates[i];
                    if (ReferenceEquals(certificate, selected) || certificate.Thumbprint == selected.Thumbprint)
                        continue;

                    if (certificate.Issuer == certificate.Subject || certificate.HasPrivateKey ||
                        !IsCertificateAuthority(certificate))
                        continue;

                    candidates.Add(certificate);
                }

                if (candidates.Count == 0)
                    return;

                using var store = new X509Store(StoreName.CertificateAuthority, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadWrite);
                existing = store.Certificates;
                foreach (var candidate in candidates)
                {
                    using var publicCopy = new X509Certificate2(candidate.RawData);
                    if (!existing.Contains(publicCopy))
                        store.Add(publicCopy);
                }
            }
            catch (Exception)
            {
                // best-effort: never fail certificate loading because of a store side effect
                Trace.TraceWarning(TracePrefix + IntermediateStoreWriteFailed);
            }
            finally
            {
                if (existing != null)
                    DisposeAll(existing, null);
            }
        }

#endif

        /// <summary>
        ///     Load public certificate from chain
        /// </summary>
        /// <param name="certificateFile">Certificate file path</param>
        /// <returns></returns>
        /// <remarks>The first certificate is returned; the others are released. Nothing is written to a certificate store.</remarks>
        private static X509Certificate2 LoadPublicCertificateFromChain(string certificateFile)
        {
            if (!File.Exists(certificateFile))
                return null;

            var x509Certificate2Collection = LoadPublicCertificatesChain(certificateFile);
            if (x509Certificate2Collection.Count.IsZero())
                return null;

            for (var i = 1; i < x509Certificate2Collection.Count; i++)
                x509Certificate2Collection[i].Reset();

            return x509Certificate2Collection[0];
        }

        /// <summary>
        ///     Load public certificate from chain
        /// </summary>
        /// <param name="certificateFile">Certificate file path</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static X509Certificate2Collection LoadPublicCertificatesChain(string certificateFile)
        {
            var x509Certificate2Collection = new X509Certificate2Collection();
            var array = File.ReadAllLines(certificateFile);
            var num = -1;
            try
            {
                for (var i = 0; i < array.Length; i++)
                {
                    if (num.IsLessZero())
                    {
                        if (array[i].Contains("BEGIN CERTIFICATE", StringComparison.OrdinalIgnoreCase)) num = i + 1;
                    }
                    else if (array[i].Contains("END CERTIFICATE", StringComparison.OrdinalIgnoreCase))
                    {
                        x509Certificate2Collection.Add(
                            new X509Certificate2(Convert.FromBase64String(string.Concat(array.SubArray(num, i - num)))));
                        num = -1;
                    }
                }
            }
            catch
            {
                for (var i = 0; i < x509Certificate2Collection.Count; i++)
                    x509Certificate2Collection[i].Reset();

                throw;
            }

            return x509Certificate2Collection;
        }
    }
}