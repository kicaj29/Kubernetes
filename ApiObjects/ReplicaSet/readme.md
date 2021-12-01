# Commands

```
kubectl apply -f nginx-rs.yaml
```
```
PS D:\GitHub\kicaj29\Kubernetes\ReplicaSet> kubectl get po -l app=nginx-app
NAME             READY   STATUS    RESTARTS   AGE
nginx-rs-7786l   1/1     Running   0          25s
nginx-rs-nc68x   1/1     Running   0          25s
nginx-rs-z8gfz   1/1     Running   0          25s
```
```
PS D:\GitHub\kicaj29\Kubernetes\ReplicaSet> kubectl get rs nginx-rs -o wide
NAME       DESIRED   CURRENT   READY   AGE   CONTAINERS        IMAGES   SELECTOR
nginx-rs   3         3         3       56s   nginx-container   nginx    app=nginx-app,tier in (frontend)
```

* Scale up
```
PS D:\GitHub\kicaj29\Kubernetes\ReplicaSet> kubectl scale rs nginx-rs --replicas=5
replicaset.apps/nginx-rs scaled
PS D:\GitHub\kicaj29\Kubernetes\ReplicaSet> kubectl get rs nginx-rs -o wide
NAME       DESIRED   CURRENT   READY   AGE     CONTAINERS        IMAGES   SELECTOR
nginx-rs   5         5         3       2m28s   nginx-container   nginx    app=nginx-app,tier in (frontend)
PS D:\GitHub\kicaj29\Kubernetes\ReplicaSet> kubectl get po -o wide
NAME             READY   STATUS    RESTARTS   AGE     IP          NODE             NOMINATED NODE   READINESS GATES
nginx-rs-2qklt   1/1     Running   0          17s     10.1.0.40   docker-desktop   <none>           <none>
nginx-rs-7786l   1/1     Running   0          2m34s   10.1.0.38   docker-desktop   <none>           <none>
nginx-rs-nc68x   1/1     Running   0          2m34s   10.1.0.36   docker-desktop   <none>           <none>
nginx-rs-xqmpt   1/1     Running   0          17s     10.1.0.39   docker-desktop   <none>           <none>
nginx-rs-z8gfz   1/1     Running   0          2m34s   10.1.0.37   docker-desktop   <none>           <none>
```

* Cleanup
```
kubectl delete -f nginx-rs.yaml
kubectl get rs
kubectl get po -l app=nginx-app
```

* Labels validation   
If we will create RS with invalid labels then an error is generated.
It means that between matchLabels and matchExpressions is operator AND.
```yaml
apiVersion: apps/v1
kind: ReplicaSet
metadata:
  name: nginx-rs
spec:
  replicas: 3
  selector:
    matchLabels:
      app: nginx-app
    matchExpressions:
      - {key: tier, operator: In, values: [frontend]}  
  template:
    metadata:
      name: nginx-pod
      labels:
        app: nginx-app
        tier: frontend1
    spec:
      containers:
      - name: nginx-container
        image: nginx
        ports:
        - containerPort: 80
```

```
PS D:\GitHub\kicaj29\Kubernetes\ReplicaSet> kubectl apply -f nginx-rs.yaml
The ReplicaSet "nginx-rs" is invalid: spec.template.metadata.labels: Invalid value: map[string]string{"app":"nginx-app", "tier":"frontend1"}: `selector` does not match template `labels`
```