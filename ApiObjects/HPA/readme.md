# Introduction - installation of metrics server

https://dev.to/docker/enable-kubernetes-metrics-server-on-docker-desktop-5434

To use HPA we have to install metrics server because it is not installed together with K8s.

We can check if the metrics server is installed using the following command:
```
PS D:\GitHub\kicaj29\Kubernetes\ApiObjects\HPA> kubectl get deployment metrics-server -n kube-system
Error from server (NotFound): deployments.apps "metrics-server" not found
```

https://github.com/kubernetes-sigs/metrics-server/releases

>NOTE: it is possible to install metrics server like this `kubectl apply -f https://github.com/kubernetes-sigs/metrics-server/releases/download/v0.5.2/components.yaml` but I had to add `kubelet-insecure-tls` param to this file so I use my own version.

Installation:
```
PS D:\GitHub\kicaj29\Kubernetes\ApiObjects\HPA> kubectl apply -f metrics-server-components.yaml
serviceaccount/metrics-server created
clusterrole.rbac.authorization.k8s.io/system:aggregated-metrics-reader created
clusterrole.rbac.authorization.k8s.io/system:metrics-server created
rolebinding.rbac.authorization.k8s.io/metrics-server-auth-reader created
clusterrolebinding.rbac.authorization.k8s.io/metrics-server:system:auth-delegator created
clusterrolebinding.rbac.authorization.k8s.io/system:metrics-server created
service/metrics-server created
deployment.apps/metrics-server created
apiservice.apiregistration.k8s.io/v1beta1.metrics.k8s.io created
```

And just in case we can check if the pod is ready (wait 10-15 secs):
```
PS D:\GitHub\kicaj29\Kubernetes\ApiObjects\HPA> kubectl get deployment metrics-server -n kube-system
NAME             READY   UP-TO-DATE   AVAILABLE   AGE
metrics-server   1/1     1            1           39s
PS D:\GitHub\kicaj29\Kubernetes\ApiObjects\HPA> kubectl get pods -n kube-system
NAME                                     READY   STATUS    RESTARTS   AGE
coredns-558bd4d5db-bv6c2                 1/1     Running   2          12d
coredns-558bd4d5db-hcxks                 1/1     Running   2          12d
etcd-docker-desktop                      1/1     Running   2          12d
kube-apiserver-docker-desktop            1/1     Running   2          12d
kube-controller-manager-docker-desktop   1/1     Running   2          12d
kube-proxy-jhbgx                         1/1     Running   2          12d
kube-scheduler-docker-desktop            1/1     Running   2          12d
metrics-server-5b6dd75459-gzfg4          1/1     Running   0          34s
storage-provisioner                      1/1     Running   4          12d
vpnkit-controller                        1/1     Running   603        12d
```
Another way to check if metrics server is running is to execute commands:

```
S D:\GitHub\kicaj29\Kubernetes\ApiObjects\HPA> kubectl top node
W1202 15:21:02.887019   34192 top_node.go:119] Using json format to get metrics. Next release will switch to protocol-buffers, switch early by passing --use-protocol-buffers flag
NAME             CPU(cores)   CPU%   MEMORY(bytes)   MEMORY%
docker-desktop   149m         1%     2602Mi          10%
PS D:\GitHub\kicaj29\Kubernetes\ApiObjects\HPA> kubectl top pod 
W1202 15:21:08.464104   28492 top_pod.go:140] Using json format to get metrics. Next release will switch to protocol-buffers, switch early by passing --use-protocol-buffers flag
NAME                          CPU(cores)   MEMORY(bytes)   
fluentd-ds-rfrw8              2m           144Mi
php-apache-598bff4655-hstfg   1m           8Mi
```

# Deploy container and HPA

* Deploy HPA and the container

```
PS D:\GitHub\kicaj29\Kubernetes\ApiObjects\HPA> kubectl apply -f hpa.yaml 
deployment.apps/php-apache created
service/php-apache created
horizontalpodautoscaler.autoscaling/php-apache created
```

* Check how many pods are running
```
PS D:\GitHub\kicaj29\Kubernetes\ApiObjects\HPA> kubectl get pods
NAME                          READY   STATUS    RESTARTS   AGE
fluentd-ds-rfrw8              1/1     Running   0          19h
php-apache-598bff4655-hstfg   1/1     Running   0          2m21s
```

* Check HPA, we have to wait few mins to see concrete value in TARGETS column (in my case even after 1 hour I still `<unknown>` was displayed)
```
PS D:\GitHub\kicaj29\Kubernetes\ApiObjects\HPA> kubectl get hpa 
NAME         REFERENCE               TARGETS         MINPODS   MAXPODS   REPLICAS   AGE
php-apache   Deployment/php-apache   <unknown>/50%   1         10        0          3m24s
```

# Testing - creating a load

Using commands from this page: https://kubernetes.io/docs/tasks/run-application/horizontal-pod-autoscale-walkthrough/#increase-load/ we will generate a load:
After few seconds it will start sending OK!OK!OK!OK!OK!OK!OK!.
```
kubectl run -i --tty load-generator --rm --image=busybox --restart=Never -- /bin/sh -c "while sleep 0.01; do wget -q -O- http://php-apache; done"
```

We can see that CPU usage in the pod is increased:

```
PS C:\Users\jkowalski> kubectl top pod
W1202 16:37:20.114935   13256 top_pod.go:140] Using json format to get metrics. Next release will switch to protocol-buffers, switch early by passing --use-protocol-buffers flag
NAME                          CPU(cores)   MEMORY(bytes)
fluentd-ds-rfrw8              2m           144Mi
load-generator                17m          0Mi
php-apache-598bff4655-hstfg   875m         11Mi
```

but new pods have not been created because from some unknown reasons that TARGET in HPA was still unknown:

```
PS D:\GitHub\kicaj29\Kubernetes\ApiObjects\HPA> kubectl get hpa
NAME         REFERENCE               TARGETS         MINPODS   MAXPODS   REPLICAS   AGE
php-apache   Deployment/php-apache   <unknown>/50%   1         10        0          3h19m
```


# Cleanup

```
kubectl delete -f metrics-server-components.yaml
kubectl delete -f hpa.yaml
```
