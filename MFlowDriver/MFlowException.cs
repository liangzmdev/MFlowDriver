using System;

namespace MFlowDriver
{
    public class MFlowException : Exception
    {
        public MFlowException(string description) : base(description)
        {
        }

        public static MFlowException Of(string description)
        {
            return new MFlowException(description);
        }

        public static MFlowException Of()
        {
            return new MFlowException("流程驱动异常");
        }
    }
}