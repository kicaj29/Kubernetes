# Intro

A typical pdb looks like:
```
apiVersion: policy/v1
kind: PodDisruptionBudget
metadata:
  name: pdb
spec:
  minAvailable: 1
  selector:
    matchLabels:
      app: nginx
```

# Voluntary Disruption

You may notice that I’ve mentioned that pdb is only good for voluntary disruption.

Voluntary disruptions can take the form of:

A node group replacement, from an incompatible change or a cluster upgrade.
Scaling up/down nodes.
Oftentimes, the responsibility of managing an application workload is separated from the responsibility of managing the cluster, and usually picked up by separate teams such as a platform team and an application team.

There can be a conflict of interest between them:

An application team wants their apps running all the times, with 100% availability and the endpoints as responsive as possible.
A platform team needs to make changes to the cluster. Those changes will take down nodes, with the pods running on them as well.
A pdb is, in all fairness, a compromise between an application team and a platform team. Application team acknowledges the necessity of having scheduled/voluntary disruption, and provides a guideline to assist in completing the rollout, which is carried out by the platform team.

Of course, there are involuntary disruptions as well, such as electricity outage or node kernel crash. pdb won’t protect your workload from them, understandably.

# Links
https://innablr.com.au/blog/what-is-pod-disruption-budget-in-k8s-and-why-to-use-it/