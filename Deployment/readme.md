# Introduction

Deployment is about updates and rollbacks.

## Deployment types

* Recreate: shutting down version A and next deploying version B, in this approach we have downtime
* RollingUpdate (ramped or incremental): new pod is added, old pod is removed, there is no downtime - **it is default deployment type**
* Canary: partial upgrade - some pods are new and some are old, if tests run against the new pods are ok we upgrade all pods to the new version, there is no downtime
* Blue/Green: green is version B, blue is version A and both versions are deployed, every version has the same amount of pods. After testing new version B all traffic is switched from version A to version B. Thx to this we can upgrade (switch) the versions instantly on the load balancer.

# Commands

## Deploy first version

```
kubectl apply -f nginx-deploy.yaml
```
```
PS D:\GitHub\kicaj29\Kubernetes\Deployment> kubectl get pods
NAME                            READY   STATUS    RESTARTS   AGE
nginx-deploy-6c5c5c6946-g7lpr   1/1     Running   0          82s
nginx-deploy-6c5c5c6946-g9lm9   1/1     Running   0          82s
nginx-deploy-6c5c5c6946-mp7zf   1/1     Running   0          82s
```
```
PS D:\GitHub\kicaj29\Kubernetes\Deployment> kubectl get rs -l app=nginx-app
NAME                      DESIRED   CURRENT   READY   AGE
nginx-deploy-6c5c5c6946   3         3         3       2m20s
```
```
S D:\GitHub\kicaj29\Kubernetes\Deployment> kubectl get deploy -l app=nginx-app
NAME           READY   UP-TO-DATE   AVAILABLE   AGE
nginx-deploy   3/3     3            3           2m34s
```
```
PS D:\GitHub\kicaj29\Kubernetes> kubectl rollout status deployment/nginx-deploy
deployment "nginx-deploy" successfully rolled out
```
```
PS D:\GitHub\kicaj29\Kubernetes> kubectl get deploy                            
NAME           READY   UP-TO-DATE   AVAILABLE   AGE 
nginx-deploy   3/3     3            3           122m
```

## Upgrade  nginx:1.19.2 -> nginx:1.20

Update the yaml file from 1.19.2 to 1.20
```
PS D:\GitHub\kicaj29\Kubernetes\Deployment> kubectl apply -f nginx-deploy.yaml
deployment.apps/nginx-deploy configured
```
Next in column UP-TO-DATE we can see how many pods have been already upgraded:
```
PS D:\GitHub\kicaj29\Kubernetes\Deployment> kubectl get deploy
NAME           READY   UP-TO-DATE   AVAILABLE   AGE
nginx-deploy   3/3     1            3           125m
PS D:\GitHub\kicaj29\Kubernetes\Deployment> kubectl get deploy
NAME           READY   UP-TO-DATE   AVAILABLE   AGE
nginx-deploy   3/3     2            3           125m
PS D:\GitHub\kicaj29\Kubernetes\Deployment> kubectl get deploy  
NAME           READY   UP-TO-DATE   AVAILABLE   AGE
nginx-deploy   3/3     3            3           126m
```

## Rollback

See the history:
```
PS D:\GitHub\kicaj29\Kubernetes\Deployment> kubectl rollout history deploy nginx-deploy
deployment.apps/nginx-deploy 
REVISION  CHANGE-CAUSE
1         <none>
2         <none>
```

next we can rollback
```
PS D:\GitHub\kicaj29\Kubernetes\Deployment> kubectl rollout undo deploy nginx-deploy --to-revision=1
deployment.apps/nginx-deploy rolled back
PS D:\GitHub\kicaj29\Kubernetes\Deployment> kubectl rollout history deploy nginx-deploy
deployment.apps/nginx-deploy 
REVISION  CHANGE-CAUSE
2         <none>
3         <none>
```