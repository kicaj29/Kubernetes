# Masters
Is Kubernates control plane.
![Master](images/master.png)

# Nodes
There are 3 main elements in a node: **Kubelet**, **container engine**, **kube-proxy**.

Node is component where **Kubelet** (Kubernetes agent) is installed. We can say that Kubelet is a node. It registers node with cluster and reports its status to master. Using port :10255 we can inspect the kubelet (/spec, /healthz, /pods).   
Other 2 main elements are **container engine** and **kube-proxy** e.g. it takes care that node has IP address (all containers in a pod share single IP).

# Pods
Pod is atomic unit of scheduling. A Pod always runs on a Node. A Node can have multiple pods. It is like a sandbox that runs containers in itself.  
**All containers in pod share the pod environment.** If there is uses case when 2 services have to be tight coupled then they should be placed in the same pod, if not then they should be placed in separated pods.

To scale up/down Kubernetes control ammount of pods and not containers inside pod.
![pods-scaling](images/pods-scaling.png)

**Pods do not support resurrection. Every time new pod is set up it is completely new pod.**

# Services
Services offers fixed IP address, DNS name and load balancing. It is needed becasue new pods get different IP addresses every time.

**Pods are assigned to a service via labels.**

![Service](images/service.png)

Other important points:

1. Service only send to healhy pods.
2. Service can be configured for session affinity.
3. Service can point to things outside the cluster.
4. Random load balancing.
5. Uses TCP by default.

# Deployments
Deployments are described via YAML or JSON manifest file. They are deployed via *apiserver* from master.

**Deployments are a newer and higher level concept than Replication Controllers.**

# links
https://app.pluralsight.com/library/courses/getting-started-kubernetes