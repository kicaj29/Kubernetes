# Introduction

Typical usage: it helps to deploy example **only one pod** on every node or a subset of nodes inside a cluster.

"
DaemonSets manage groups of replicated Pods. However, DaemonSets attempt to adhere to a one-Pod-per-node model, either across the entire cluster or a subset of nodes. A Daemonset will not run more than one replica per node. Another advantage of using a Daemonset is that, if you add a node to the cluster, then the Daemonset will automatically spawn a pod on that node, which a deployment will not do.
"

More https://stackoverflow.com/questions/53888389/difference-between-daemonsets-and-deployments/53888573#53888573

# Commands

```
PS D:\GitHub\kicaj29\Kubernetes\ApiObjects\DeamonSet> kubectl apply -f fluentd-ds-allnodes.yaml
daemonset.apps/fluentd-ds created
PS D:\GitHub\kicaj29\Kubernetes\ApiObjects\DeamonSet> kubectl get po -o wide
NAME               READY   STATUS    RESTARTS   AGE   IP          NODE             NOMINATED NODE   READINESS GATES
fluentd-ds-rfrw8   1/1     Running   0          78s   10.1.0.67   docker-desktop   <none>           <none>
```