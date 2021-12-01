# Creating secrets

Secrets can be created using yaml files or via CLI.
If we use CLI then we do not have to pass base64 values because it is encoded automatically by the command.

>NOTE: by default secrets are not encrypted in K8s, more here: https://kubernetes.io/docs/concepts/configuration/secret/

Using file:
```
kubectl apply -f my-secrets.yaml
```
Using CLI:
```
PS D:\GitHub\kicaj29\Kubernetes\Secrets> kubectl create secret generic my-cli-secret --from-literal=API_TOKEN_CLI=password123
secret/my-cli-secret created
PS D:\GitHub\kicaj29\Kubernetes\Secrets> kubectl get secret/my-cli-secret
NAME            TYPE     DATA   AGE
my-cli-secret   Opaque   1      16s
```

# Using secrets

```
kubectl apply -f sample-pod.yaml
```
We can see that here the secret is a raw value already encoded from base64
```
kubectl logs pod1
apiToken=password123
```

Also we can see that secrets used via mounting worked correctly

```
PS D:\GitHub\kicaj29\Kubernetes>  kubectl exec -it pod1 -- /bin/sh
/ # ls
bin     config  dev     etc     home    lib     media   mnt     opt     proc    root    run     sbin    srv     sys     tmp     usr     var
/ # cd config
/config # ls
API_TOKEN_CLI
/config # cat API_TOKEN_CLI
password123/config # 
```
