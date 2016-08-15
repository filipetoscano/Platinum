using System;
using System.IO;

namespace Platinum.IO
{
    public static class FsPath
    {
        /// <summary>
        /// Canonicalizes the path, removing any relative navigations.
        /// </summary>
        /// <param name="path">
        /// Path to be canonicalized.
        /// </param>
        /// <returns>
        /// Canonicalized path.
        /// </returns>
        public static string Canonicalize( string path )
        {
            #region Validations

            if ( path == null )
                throw new ArgumentNullException( nameof( path ) );

            #endregion

            string originalRoot = string.Empty;

            if ( Path.IsPathRooted( path ) == true )
            {
                originalRoot = Path.GetPathRoot( path );
                path = path.Substring( originalRoot.Length );
            }

            string fakeRoot = @"\\fake\root\";
            path = Path.Combine( fakeRoot, path );
            path = Path.GetFullPath( path );

            path = path.Substring( fakeRoot.Length );
            path = Path.Combine( originalRoot, path );

            return path;
        }


        /// <summary>
        /// Combines an array of strings into a path.
        /// </summary>
        /// <param name="paths">
        /// An array of parts of the path.
        /// </param>
        /// <returns>
        /// The combined paths, canonicalized.
        /// </returns>
        public static string Combine( params string[] paths )
        {
            string c = Path.Combine( paths );

            return Canonicalize( c );
        }


        /// <summary>
        /// Combines two strings into a path.
        /// </summary>
        /// <param name="path1">
        /// The first path to combine.
        /// </param>
        /// <param name="path2">
        /// The second path to combine.
        /// </param>
        /// <returns>
        /// The combined paths, canonicalized. If one of the specified paths is a
        /// zero-length string, this method returns the other path. If path2
        /// contains an absolute path, this method returns path2.
        /// </returns>
        public static string Combine( string path1, string path2 )
        {
            string c = Path.Combine( path1, path2 );

            return Canonicalize( c );
        }


        /// <summary>
        /// Combines three strings into a path.
        /// </summary>
        /// <param name="path1">
        /// The first path to combine.
        /// </param>
        /// <param name="path2">
        /// The second path to combine.
        /// </param>
        /// <param name="path3">
        /// The third path to combine.
        /// </param>
        /// <returns>
        /// The combined paths, canonicalized.
        /// </returns>
        public static string Combine( string path1, string path2, string path3 )
        {
            string c = Path.Combine( path1, path2, path3 );

            return Canonicalize( c );
        }


        /// <summary>
        /// Combines four strings into a path.
        /// </summary>
        /// <param name="path1">
        /// The first path to combine.
        /// </param>
        /// <param name="path2">
        /// The second path to combine.
        /// </param>
        /// <param name="path3">
        /// The third path to combine.
        /// </param>
        /// <param name="path4">
        /// The fourth parth to combine.
        /// </param>
        /// <returns>
        /// The combined paths, canonicalized.
        /// </returns>
        public static string Combine( string path1, string path2, string path3, string path4 )
        {
            string c = Path.Combine( path1, path2, path3, path4 );

            return Canonicalize( c );
        }


        /// <summary>
        /// Creates a uniquely named, zero-byte temporary file on disk and returns the full
        /// path of that file.
        /// </summary>
        /// <returns>
        /// The full path of the temporary file.
        /// </returns>
        public static string GetTempFileName()
        {
            return Path.GetTempFileName();
        }


        /// <summary>
        /// Returns the path of the current user's temporary folder.
        /// </summary>
        /// <returns>
        /// The path to the temporary folder, ending with a backslash.
        /// </returns>
        public static string GetTempPath()
        {
            return Path.GetTempPath();
        }
    }
}
