# Creating config maps
Config map can be created using CLI or using yaml file.

* Create config map using CLI

Here they key is a file name:
```ps
PS D:\GitHub\kicaj29\Kubernetes\ConfigMaps> kubectl create configmap config1 --from-file=properties.conf
configmap/config1 created
PS D:\GitHub\kicaj29\Kubernetes\ConfigMaps> kubectl get cm/config1 -o yaml
apiVersion: v1
data:
  properties.conf: "prop1=val1\r\nprop2=val2\r\nprop3=val3"
kind: ConfigMap
metadata:
  creationTimestamp: "2021-11-30T15:43:25Z"
  name: config1
  namespace: default
  resourceVersion: "336495"
  uid: f47d26ac-4e93-4984-be78-8b02e11c6c38
```

Here we can specify own key name:
```ps
PS D:\GitHub\kicaj29\Kubernetes\ConfigMaps> kubectl create configmap config2 --from-file=custom-key=properties.conf
configmap/config2 created
PS D:\GitHub\kicaj29\Kubernetes\ConfigMaps> kubectl get cm/config2 -o yaml
apiVersion: v1
data:
  custom-key: "prop1=val1\r\nprop2=val2\r\nprop3=val3"
kind: ConfigMap
metadata:
  creationTimestamp: "2021-11-30T15:47:28Z"
  name: config2
  namespace: default
  resourceVersion: "336804"
  uid: 0cee215e-cf69-4d5b-b102-d2afe3fe380b
```

Specify the keys as parameter:
```ps
PS D:\GitHub\kicaj29\Kubernetes\ConfigMaps> kubectl create configmap config3 --from-literal=the-key=its-value
configmap/config3 created
PS D:\GitHub\kicaj29\Kubernetes\ConfigMaps> kubectl get cm/config3 -o yaml
apiVersion: v1
data:
  the-key: its-value
kind: ConfigMap
metadata:
  creationTimestamp: "2021-11-30T15:50:12Z"
  name: config3
  namespace: default
  resourceVersion: "337007"
  uid: 4a5fa2cb-fff3-49b0-a9f3-f2c15940aef5
```

Mixing: using file and parameters
```ps
PS D:\GitHub\kicaj29\Kubernetes\ConfigMaps> kubectl create configmap config4 --from-literal=the-key1=its-value1 --from-literal=the-key2=its-value2 --from-file=the-key3=properties.conf
configmap/config4 created
PS D:\GitHub\kicaj29\Kubernetes\ConfigMaps> kubectl get cm/config4 -o yaml
apiVersion: v1
data:
  the-key1: its-value1
  the-key2: its-value2
  the-key3: "prop1=val1\r\nprop2=val2\r\nprop3=val3"
kind: ConfigMap
metadata:
  creationTimestamp: "2021-11-30T15:53:11Z"
  name: config4
  namespace: default
  resourceVersion: "337229"
  uid: 294d5e46-2b3f-4032-944e-d8d14f5587c7
```


* Create config map using yaml

```ps
PS D:\GitHub\kicaj29\Kubernetes\ConfigMaps> kubectl apply -f settings-cm.yaml 
configmap/settings-config created
PS D:\GitHub\kicaj29\Kubernetes\ConfigMaps> kubectl get cm/settings-config -o yaml
apiVersion: v1
data:
  bees: honey
  key1: val1
  properties: |
    prop1=val1
    prop2=val2
    prop3=val3
kind: ConfigMap
metadata:
  annotations:
    kubectl.kubernetes.io/last-applied-configuration: |
      {"apiVersion":"v1","data":{"bees":"honey","key1":"val1","properties":"prop1=val1\nprop2=val2\nprop3=val3\n"},"kind":"ConfigMap","metadata":{"annotations":{},"name":"settings-config","namespace":"default"}}
  creationTimestamp: "2021-11-30T16:00:09Z"
  name: settings-config
  namespace: default
  resourceVersion: "339762"
  uid: d8c63753-fb3f-4a57-abc1-0287af68baf9
```

# ConfigMaps for environment variables

For environment variables we use key:value section from the config map.

# ConfigMaps for creating files

For mounting files we use part of the config map where is used |.
It is typically used to use create dedicated config files for the processes run in the container.

# Example

```
kubectl apply -f settings-cm.yaml 
```
```
kubectl apply -f sample-pod.yaml
```
To see values printed by `echo` command:
```
kubectl logs pod1
```
Next we can check that the files has been created:
```
PS D:\GitHub\kicaj29\Kubernetes> kubectl exec -it pod1 -- /bin/sh
/ # ls
bin     config  dev     etc     home    lib     media   mnt     opt     proc    root    run     sbin    srv     sys     tmp     usr     var
/ # cd config
/config # ls
database.conf    mySettings.conf
/config # cat database.conf
dbsetting1=dbvalue1
dbsetting2=dbvalue2
dbsetting2=dbvalue3
/config # cat mySettings.conf
setting1=value1
setting2=value2
setting3=value3
/config # exit
```
```
kubectl delete pod pod1
```

# Links
https://kubernetes.io/docs/concepts/configuration/configmap/   
https://www.bmc.com/blogs/kubernetes-configmap/