using System;

namespace MFlowDriver
{
    /// <summary>
    /// MFlowException
    /// </summary>
    public class MFlowException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="description"></param>
        public MFlowException(string description) : base(description)
        {
        }

        /// <summary>
        /// Of
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static MFlowException Of(string description)
        {
            return new MFlowException(description);
        }

        /// <summary>
        /// Of
        /// </summary>
        /// <returns></returns>
        public static MFlowException Of()
        {
            return new MFlowException("流程驱动异常");
        }
    }
}