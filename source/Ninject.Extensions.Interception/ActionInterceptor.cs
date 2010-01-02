#region License

// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using System;

#endregion

namespace Ninject.Extensions.Interception
{
    public class ActionInterceptor : IInterceptor
    {
        private readonly Action<IInvocation> _interceptAction;

        public ActionInterceptor( Action<IInvocation> interceptAction )
        {
            _interceptAction = interceptAction;
        }

        #region IInterceptor Members

        public void Intercept( IInvocation invocation )
        {
            _interceptAction( invocation );
        }

        #endregion
    }
}