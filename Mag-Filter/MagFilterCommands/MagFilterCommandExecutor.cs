using System;

using Mag.Shared;

namespace MagFilter
{
    class MagFilterCommandExecutor
    {
        public void ExecuteCommand(string command)
        {
            if (!string.IsNullOrEmpty(command))
            {
                DecalProxy.DispatchChatToBoxWithPluginIntercept(command);
            }
        }
    }
}
