# Commands

```
kubectl apply -f nginx-rc.yaml 
```

```
PS D:\GitHub\kicaj29\Kubernetes\ReplicationController> kubectl get po -o wide
NAME             READY   STATUS    RESTARTS   AGE   IP          NODE             NOMINATED NODE   READINESS GATES
nginx-rc-4c67w   1/1     Running   0          86s   10.1.0.33   docker-desktop   <none>           <none>
nginx-rc-4gdrm   1/1     Running   0          86s   10.1.0.31   docker-desktop   <none>           <none>
nginx-rc-twlzp   1/1     Running   0          86s   10.1.0.32   docker-desktop   <none>           <none>
PS D:\GitHub\kicaj29\Kubernetes\ReplicationController> kubectl get po -l app=nginx-app
NAME             READY   STATUS    RESTARTS   AGE
nginx-rc-4c67w   1/1     Running   0          2m23s
nginx-rc-4gdrm   1/1     Running   0          2m23s
nginx-rc-twlzp   1/1     Running   0          2m23s
PS D:\GitHub\kicaj29\Kubernetes\ReplicationController> kubectl get rc nginx-rc
NAME       DESIRED   CURRENT   READY   AGE
nginx-rc   3         3         3       2m31s
```

Scaling up:
```
PS D:\GitHub\kicaj29\Kubernetes\ReplicationController> kubectl scale rc nginx-rc --replicas=5
replicationcontroller/nginx-rc scaled
PS D:\GitHub\kicaj29\Kubernetes\ReplicationController> kubectl get rc nginx-rc
NAME       DESIRED   CURRENT   READY   AGE
nginx-rc   5         5         3       7m1s
PS D:\GitHub\kicaj29\Kubernetes\ReplicationController> kubectl get po -o wide
NAME             READY   STATUS    RESTARTS   AGE    IP          NODE             NOMINATED NODE   READINESS GATES
nginx-rc-4c67w   1/1     Running   0          7m7s   10.1.0.33   docker-desktop   <none>           <none>
nginx-rc-4gdrm   1/1     Running   0          7m7s   10.1.0.31   docker-desktop   <none>           <none>
nginx-rc-9bg5f   1/1     Running   0          11s    10.1.0.34   docker-desktop   <none>           <none>
nginx-rc-gfhzp   1/1     Running   0          11s    10.1.0.35   docker-desktop   <none>           <none>
nginx-rc-twlzp   1/1     Running   0          7m7s   10.1.0.32   docker-desktop   <none>           <none>
```

Cleanup:
```
kubectl delete -f nginx-rc.yaml
kubectl get rc
kubectl get po -l app=nginx-app
```