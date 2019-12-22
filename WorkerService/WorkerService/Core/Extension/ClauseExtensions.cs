using System;

namespace WorkerService.Core.Extension
{
    public static class ClauseExtensions
    {
        /// <summary>
        /// Throws an <see cref="ArgumentNullException" /> if <see cref="input" /> is null.
        /// Throws an <see cref="ArgumentException" /> if <see cref="input" /> is an empty or white space string.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="parameterName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static void NullOrWhiteSpace(this string input, string parameterName)
        {
            NullOrEmpty(input, parameterName);
            if (String.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException($"Required input {parameterName} was empty.", parameterName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException" /> if <see cref="input" /> is null.
        /// Throws an <see cref="ArgumentException" /> if <see cref="input" /> is an empty string.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="parameterName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static void NullOrEmpty(this string input, string parameterName)
        {
            Null(input, parameterName);
            if (input == String.Empty)
            {
                throw new ArgumentException($"Required input {parameterName} was empty.", parameterName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException" /> if <see cref="input" /> is null.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="parameterName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Null(this object input, string parameterName)
        {
            if (null == input)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}
