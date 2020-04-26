using System;
using System.Collections.Generic;
using Docnet.Core.Editors;
using Docnet.Core.Models;
using Docnet.Core.Readers;

namespace Docnet.Core
{
    /// <inheritdoc />
    /// <summary>
    /// DocNet library object.
    /// Should be long lived and only
    /// disposed once.
    /// </summary>
    public interface IDocLib : IDisposable
    {
        /// <summary>
        /// Get document reader for this particular document.
        /// </summary>
        /// <param name="filePath">Full file path</param>
        /// <param name="dimensionOptions">Page scaling options</param>
        /// <returns>Document reader object</returns>
        IDocReader GetDocReader(string filePath, PageDimensions dimensionOptions);

        /// <summary>
        /// Get document reader for this particular document.
        /// </summary>
        /// <param name="filePath">Full file path</param>
        /// <param name="password">File password</param>
        /// <param name="dimensionOptions">Page scaling options</param>
        /// <returns>Document reader object</returns>
        IDocReader GetDocReader(string filePath, string password, PageDimensions dimensionOptions);

        /// <summary>
        /// Get document reader for this particular document.
        /// </summary>
        /// <param name="bytes">File bytes</param>
        /// <param name="dimensionOptions">Page scaling options</param>
        /// <returns>Document reader object</returns>
        IDocReader GetDocReader(byte[] bytes, PageDimensions dimensionOptions);

        /// <summary>
        /// Get document reader for this particular document.
        /// </summary>
        /// <param name="bytes">File bytes</param>
        /// <param name="password">File password</param>
        /// <param name="dimensionOptions">Page scaling options</param>
        /// <returns>Document reader object</returns>
        IDocReader GetDocReader(byte[] bytes, string password, PageDimensions dimensionOptions);

        /// <summary>
        /// Merge two documents into one.
        /// </summary>
        /// <param name="fileOne">Full file path one</param>
        /// <param name="fileTwo">Full file path two</param>
        /// <returns>New file bytes</returns>
        byte[] Merge(string fileOne, string fileTwo);

        /// <summary>
        /// Merge two documents into one.
        /// </summary>
        /// <param name="fileOne">File one bytes</param>
        /// <param name="fileTwo">File two bytes</param>
        /// <returns>New file bytes</returns>
        byte[] Merge(byte[] fileOne, byte[] fileTwo);

        /// <summary>
        /// Split a range of pages into a separate document.
        /// </summary>
        /// <param name="filePath">Full file path</param>
        /// <param name="pageFromIndex">Zero based page index</param>
        /// <param name="pageToIndex">Zero based page index</param>
        /// <returns>New file bytes</returns>
        byte[] Split(string filePath, int pageFromIndex, int pageToIndex);

        /// <summary>
        /// Split a range of pages into a separate document.
        /// </summary>
        /// <param name="bytes">File bytes</param>
        /// <param name="pageFromIndex">Zero based page index</param>
        /// <param name="pageToIndex">Zero based page index</param>
        /// <returns>New file bytes</returns>
        byte[] Split(byte[] bytes, int pageFromIndex, int pageToIndex);

        /// <summary>
        /// Split a range(s) of pages into a separate document.
        /// </summary>
        /// <param name="filePath">Full file path</param>
        /// <param name="pageRange">NOT 0-based! i.e. 1,3,5-7. String 1,1,1 - will generate 3-page document.</param>
        byte[] Split(string filePath, string pageRange);

        /// <summary>
        /// Split a range(s) of pages into a separate document.
        /// </summary>
        /// <param name="bytes">File bytes</param>
        /// <param name="pageRange">NOT 0-based! i.e. 1,3,5-7. String 1,1,1 - will generate 3-page document.</param>
        byte[] Split(byte[] bytes, string pageRange);

        /// <summary>
        /// Unlock a given document.
        /// </summary>
        /// <param name="filePath">Full file path</param>
        /// <param name="password">File password</param>
        /// <returns>New file bytes</returns>
        byte[] Unlock(string filePath, string password);

        /// <summary>
        /// Unlock a given document.
        /// </summary>
        /// <param name="bytes">File bytes</param>
        /// <param name="password">File password</param>
        /// <returns>New file bytes</returns>
        byte[] Unlock(byte[] bytes, string password);

        /// <summary>
        /// Convert JPEG files to PDF
        /// </summary>
        /// <param name="files">JPEG byte arrays</param>
        /// <returns>New PDF file bytes</returns>
        byte[] JpegToPdf(IReadOnlyList<JpegImage> files);

        /// <summary>
        /// Get a description of the last error
        /// that has occured.
        /// </summary>
        /// <returns>Error message</returns>
        string GetLastError();
    }
}