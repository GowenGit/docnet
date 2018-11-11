using System;
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
        /// dimOne x dimTwo represents a viewport to which
        /// the document gets scaled to fit without modifying
        /// it's aspect ratio.
        /// </summary>
        /// <param name="filePath">Full file path</param>
        /// <param name="dimOne">smaller viewport dimension</param>
        /// <param name="dimTwo">larger viewport dimension</param>
        /// <returns>Document reader object</returns>
        IDocReader GetDocReader(string filePath, int dimOne, int dimTwo);

        /// <summary>
        /// Get document reader for this particular document.
        /// dimOne x dimTwo represents a viewport to which
        /// the document gets scaled to fit without modifying
        /// it's aspect ratio.
        /// </summary>
        /// <param name="filePath">Full file path</param>
        /// <param name="password">File password</param>
        /// <param name="dimOne">smaller viewport dimension</param>
        /// <param name="dimTwo">larger viewport dimension</param>
        /// <returns>Document reader object</returns>
        IDocReader GetDocReader(string filePath, string password, int dimOne, int dimTwo);

        /// <summary>
        /// Merge two documents into one.
        /// </summary>
        /// <param name="fileOne">Full file path one</param>
        /// <param name="fileTwo">Full file path two</param>
        /// <returns>New file bytes</returns>
        byte[] Merge(string fileOne, string fileTwo);

        /// <summary>
        /// Split a range of pages into a separate document.
        /// </summary>
        /// <param name="filePath">Full file path</param>
        /// <param name="pageFromIndex">Zero based page index</param>
        /// <param name="pageToIndex">Zero based page index</param>
        /// <returns>New file bytes</returns>
        byte[] Split(string filePath, int pageFromIndex, int pageToIndex);

        /// <summary>
        /// Unlock a given document.
        /// </summary>
        /// <param name="filePath">Full file path</param>
        /// <param name="password">File password</param>
        /// <returns>New file bytes</returns>
        byte[] Unlock(string filePath, string password);
    }
}