using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace EXAM2
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        private ServiceInstaller _serviceInstaller;
        private ServiceProcessInstaller _processInstaller;

        public ProjectInstaller()
        {
            _processInstaller = new ServiceProcessInstaller();
            _serviceInstaller = new ServiceInstaller();

            // Set up the process installer
            _processInstaller.Account = ServiceAccount.LocalService;

            // Set up the service installer
            _serviceInstaller.StartType = ServiceStartMode.Automatic;
            _serviceInstaller.ServiceName = "FileMoverService";
            _serviceInstaller.DisplayName = "File Mover Service";

            Installers.Add(_processInstaller);
            Installers.Add(_serviceInstaller);
        }
    }
}
