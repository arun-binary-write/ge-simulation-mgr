using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SimulationManager.Worker.Helper
{
    [ServiceContract]
    public partial interface IServiceManagement
    {
        #region Get Deployment

        /// <summary>
        /// Gets the specified deployment details.
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        [WebGet(UriTemplate = @"{subscriptionId}/services/hostedservices/{serviceName}/deployments/{deploymentName}")]
        IAsyncResult BeginGetDeployment(string subscriptionId, string serviceName, string deploymentName, AsyncCallback callback, object state);
        Deployment EndGetDeployment(IAsyncResult asyncResult);

        /// <summary>
        /// Gets the specified deployment details.
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        [WebGet(UriTemplate = @"{subscriptionId}/services/hostedservices/{serviceName}/deploymentslots/{deploymentSlot}")]
        IAsyncResult BeginGetDeploymentBySlot(string subscriptionId, string serviceName, string deploymentSlot, AsyncCallback callback, object state);
        Deployment EndGetDeploymentBySlot(IAsyncResult asyncResult);

        #endregion

        #region Change Deployment Config

        /// <summary>
        /// Initiates a change to the deployment. This works against through the deployment name.
        /// This is an asynchronous operation
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        [WebInvoke(Method = "POST", UriTemplate = @"{subscriptionId}/services/hostedservices/{serviceName}/deployments/{deploymentName}/?comp=config")]
        IAsyncResult BeginChangeConfiguration(string subscriptionId, string serviceName, string deploymentName, ChangeConfigurationInput input, AsyncCallback callback, object state);
        void EndChangeConfiguration(IAsyncResult asyncResult);

        /// <summary>
        /// Initiates a change to the deployment. This works against through the slot name.
        /// This is an asynchronous operation
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        [WebInvoke(Method = "POST", UriTemplate = @"{subscriptionId}/services/hostedservices/{serviceName}/deploymentslots/{deploymentSlot}/?comp=config")]
        IAsyncResult BeginChangeConfigurationBySlot(string subscriptionId, string serviceName, string deploymentSlot, ChangeConfigurationInput input, AsyncCallback callback, object state);
        void EndChangeConfigurationBySlot(IAsyncResult asyncResult);

        #endregion

        [OperationContract(AsyncPattern = true)]
        [WebGet(UriTemplate = @"{subscriptionId}/operations/{operationTrackingId}")]
        IAsyncResult BeginGetOperationStatus(string subscriptionId, string operationTrackingId, AsyncCallback callback, object state);
        Operation EndGetOperationStatus(IAsyncResult asyncResult);
      
    }

    public static partial class ServiceManagementExtensionMethods
    {
     
        public static Deployment GetDeployment(this IServiceManagement proxy, string subscriptionId, string serviceName, string deploymentName)
        {
            return proxy.EndGetDeployment(proxy.BeginGetDeployment(subscriptionId, serviceName, deploymentName, null, null));
        }

        public static Deployment GetDeploymentBySlot(this IServiceManagement proxy, string subscriptionId, string serviceName, string deploymentSlot)
        {
            return proxy.EndGetDeploymentBySlot(proxy.BeginGetDeploymentBySlot(subscriptionId, serviceName, deploymentSlot, null, null));
        }
     
        public static void ChangeConfiguration(this IServiceManagement proxy, string subscriptionId, string serviceName, string deploymentName, ChangeConfigurationInput input)
        {
            proxy.EndChangeConfiguration(proxy.BeginChangeConfiguration(subscriptionId, serviceName, deploymentName, input, null, null));
        }

        public static void ChangeConfigurationBySlot(this IServiceManagement proxy, string subscriptionId, string serviceName, string deploymentSlot, ChangeConfigurationInput input)
        {
            proxy.EndChangeConfigurationBySlot(proxy.BeginChangeConfigurationBySlot(subscriptionId, serviceName, deploymentSlot, input, null, null));
        }

        public static Operation GetOperationStatus(this IServiceManagement proxy, string subscriptionId, string operationId)
        {
            return proxy.EndGetOperationStatus(proxy.BeginGetOperationStatus(subscriptionId, operationId, null, null));
        }

        
    }
}
