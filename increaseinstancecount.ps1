function SetRoleInstanceCount([int] $count)
{
   
   $subscriptionId="788d90bb-b1d5-44eb-a335-aa8569d69bc6"
   $subscriptionName="Azdem155F90419I"
   $cert=Get-Item Cert:\CurrentUser\My\05E1CA7DAE8B832BFAA9C008F112F28661DC4597
   Set-AzureSubscription -SubscriptionId $subscriptionId -SubscriptionName $subscriptionName -Certificate $cert
   Select-AzureSubscription -SubscriptionName $subscriptionName
   $roleInstance=Get-AzureRole -ServiceName "gemockservice" -Slot Production -RoleName "MockService.Worker"
   
   
}

SetRoleInstanceCount 2

