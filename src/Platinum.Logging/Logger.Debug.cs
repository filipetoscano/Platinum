using NLog;
using System;
using System.ComponentModel;

namespace Platinum.Logging
{
    public partial class Logger
    {
        /// <overloads>
        /// Writes the diagnostic message at the <c>Debug</c> level using the specified format provider and format parameters.
        /// </overloads>
        /// <summary>
        /// Writes the diagnostic message at the <c>Debug</c> level.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="value">The value to be written.</param>
        public void Debug<T>( T value )
        {
            this._logger.Debug<T>( value );
        }


        /// <summary>
        /// Writes the diagnostic message at the <c>Debug</c> level.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to be written.</param>
        public void Debug<T>( IFormatProvider formatProvider, T value )
        {
            this._logger.Debug<T>( formatProvider, value );
        }


        /// <summary>
        /// Writes the diagnostic message at the <c>Debug</c> level.
        /// </summary>
        /// <param name="messageFunc">
        /// A function returning message to be written. Function is not evaluated if 
        /// logging is not enabled.
        /// </param>
        public void Debug( LogMessageGenerator messageFunc )
        {
            this._logger.Debug( messageFunc );
        }


        /// <summary>
        /// Writes the diagnostic message at the <c>Debug</c> level using the specified parameters and formatting them with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A <see langword="string" /> containing format items.</param>
        /// <param name="args">Arguments to format.</param>
        public void Debug( IFormatProvider formatProvider, [Localizable( false )] string message, params object[] args )
        {
            this._logger.Debug( formatProvider, message, args );
        }


        /// <summary>
        /// Writes the diagnostic message at the <c>Debug</c> level.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Debug( [Localizable( false )] string message )
        {
            this._logger.Debug( message );
        }


        /// <summary>
        /// Writes the diagnostic message at the <c>Debug</c> level using the specified parameters.
        /// </summary>
        /// <param name="message">A <see langword="string" /> containing format items.</param>
        /// <param name="args">Arguments to format.</param>
        public void Debug( [Localizable( false )] string message, params object[] args )
        {
            this._logger.Debug( message, args );
        }


        /// <summary>
        /// Writes the diagnostic message and exception at the <c>Debug</c> level.
        /// </summary>
        /// <param name="message">A <see langword="string" /> to be written.</param>
        /// <param name="exception">An exception to be logged.</param>
        public void Debug( Exception exception, [Localizable( false )] string message )
        {
            this._logger.Debug( exception, message );
        }


        /// <summary>
        /// Writes the diagnostic message and exception at the <c>Debug</c> level.
        /// </summary>
        /// <param name="message">A <see langword="string" /> to be written.</param>
        /// <param name="exception">An exception to be logged.</param>
        /// <param name="args">Arguments to format.</param>
        public void Debug( Exception exception, [Localizable( false )] string message, params object[] args )
        {
            this._logger.Debug( exception, message, args );
        }


        /// <summary>
        /// Writes the diagnostic message and exception at the <c>Debug</c> level.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A <see langword="string" /> to be written.</param>
        /// <param name="exception">An exception to be logged.</param>
        /// <param name="args">Arguments to format.</param>
        public void Debug( Exception exception, IFormatProvider formatProvider, [Localizable( false )] string message, params object[] args )
        {
            this._logger.Debug( exception, formatProvider, message, args );
        }


        /// <summary>
        /// Writes the diagnostic message at the <c>Debug</c> level using the specified parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A <see langword="string" /> containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        public void Debug<TArgument>( IFormatProvider formatProvider, [Localizable( false )] string message, TArgument argument )
        {
            this._logger.Debug<TArgument>( formatProvider, message, argument );
        }


        /// <summary>
        /// Writes the diagnostic message at the <c>Debug</c> level using the specified parameter.
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="message">A <see langword="string" /> containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        public void Debug<TArgument>( [Localizable( false )] string message, TArgument argument )
        {
            this._logger.Debug<TArgument>( message, argument );
        }


        /// <summary>
        /// Writes the diagnostic message at the <c>Debug</c> level using the specified arguments formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A <see langword="string" /> containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        public void Debug<TArgument1, TArgument2>( IFormatProvider formatProvider, [Localizable( false )] string message, TArgument1 argument1, TArgument2 argument2 )
        {
            this._logger.Debug<TArgument1, TArgument2>( formatProvider, message, argument1, argument2 );
        }


        /// <summary>
        /// Writes the diagnostic message at the <c>Debug</c> level using the specified parameters.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <param name="message">A <see langword="string" /> containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        public void Debug<TArgument1, TArgument2>( [Localizable( false )] string message, TArgument1 argument1, TArgument2 argument2 )
        {
            this._logger.Debug<TArgument1, TArgument2>( message, argument1, argument2 );
        }


        /// <summary>
        /// Writes the diagnostic message at the <c>Debug</c> level using the specified arguments formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <typeparam name="TArgument3">The type of the third argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A <see langword="string" /> containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        /// <param name="argument3">The third argument to format.</param>
        public void Debug<TArgument1, TArgument2, TArgument3>( IFormatProvider formatProvider, [Localizable( false )] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3 )
        {
            this._logger.Debug<TArgument1, TArgument2, TArgument3>( formatProvider, message, argument1, argument2, argument3 );
        }


        /// <summary>
        /// Writes the diagnostic message at the <c>Debug</c> level using the specified parameters.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <typeparam name="TArgument3">The type of the third argument.</typeparam>
        /// <param name="message">A <see langword="string" /> containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        /// <param name="argument3">The third argument to format.</param>
        public void Debug<TArgument1, TArgument2, TArgument3>( [Localizable( false )] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3 )
        {
            this._logger.Debug<TArgument1, TArgument2, TArgument3>( message, argument1, argument2, argument3 );
        }
    }
}
