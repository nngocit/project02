﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebComment.API.ServiceReference {
    using System.Data;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference.ServiceSoap")]
    public interface ServiceSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/DoLogin_Info", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable DoLogin_Info(string UserOrEmpID, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/DoLogin_Info", ReplyAction="*")]
        System.Threading.Tasks.Task<System.Data.DataTable> DoLogin_InfoAsync(string UserOrEmpID, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/DoLogin", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        int DoLogin(string Username, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/DoLogin", ReplyAction="*")]
        System.Threading.Tasks.Task<int> DoLoginAsync(string Username, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/FindUserLDAP", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string FindUserLDAP(string Username, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/FindUserLDAP", ReplyAction="*")]
        System.Threading.Tasks.Task<string> FindUserLDAPAsync(string Username, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/FindUserLDAPDomain", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string FindUserLDAPDomain(string Username, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/FindUserLDAPDomain", ReplyAction="*")]
        System.Threading.Tasks.Task<string> FindUserLDAPDomainAsync(string Username, string Password);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ServiceSoapChannel : WebComment.API.ServiceReference.ServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceSoapClient : System.ServiceModel.ClientBase<WebComment.API.ServiceReference.ServiceSoap>, WebComment.API.ServiceReference.ServiceSoap {
        
        public ServiceSoapClient() {
        }
        
        public ServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Data.DataTable DoLogin_Info(string UserOrEmpID, string Password) {
            return base.Channel.DoLogin_Info(UserOrEmpID, Password);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataTable> DoLogin_InfoAsync(string UserOrEmpID, string Password) {
            return base.Channel.DoLogin_InfoAsync(UserOrEmpID, Password);
        }
        
        public int DoLogin(string Username, string Password) {
            return base.Channel.DoLogin(Username, Password);
        }
        
        public System.Threading.Tasks.Task<int> DoLoginAsync(string Username, string Password) {
            return base.Channel.DoLoginAsync(Username, Password);
        }
        
        public string FindUserLDAP(string Username, string Password) {
            return base.Channel.FindUserLDAP(Username, Password);
        }
        
        public System.Threading.Tasks.Task<string> FindUserLDAPAsync(string Username, string Password) {
            return base.Channel.FindUserLDAPAsync(Username, Password);
        }
        
        public string FindUserLDAPDomain(string Username, string Password) {
            return base.Channel.FindUserLDAPDomain(Username, Password);
        }
        
        public System.Threading.Tasks.Task<string> FindUserLDAPDomainAsync(string Username, string Password) {
            return base.Channel.FindUserLDAPDomainAsync(Username, Password);
        }
    }
}
