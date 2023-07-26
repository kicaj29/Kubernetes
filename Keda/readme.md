- [Docker login](#docker-login)
- [Build docker image and test it](#build-docker-image-and-test-it)
- [Create and update helm package](#create-and-update-helm-package)
- [Verify k8s connection settings](#verify-k8s-connection-settings)
- [Verify helm package before deployment](#verify-helm-package-before-deployment)
- [Install fancy-ms chart](#install-fancy-ms-chart)
- [Call fancy-ms](#call-fancy-ms)
- [Install new version of the chart to fix problem with health checks](#install-new-version-of-the-chart-to-fix-problem-with-health-checks)
- [Call fancy-ms again](#call-fancy-ms-again)

# Docker login

```
PS D:\GitHub\kicaj29\Kubernetes\Keda\FancyMicroservice> docker login
Login with your Docker ID to push and pull images from Docker Hub. If you don't have a Docker ID, head over to https://hub.docker.com to create one.
Username: kicaj29
Password:
Login Succeeded

Logging in with your password grants your terminal complete access to your account.
For better security, log in with a limited-privilege personal access token. Learn more at https://docs.docker.com/go/access-tokens/
```

# Build docker image and test it

* Because we do not need HTTPS in this scenario and enabling it for `docker run` is a bit difficult (more [here](https://medium.com/@woeterman_94/docker-in-visual-studio-unable-to-configure-https-endpoint-f95727187f5f)) I decided to disable HTTPS. If HTTPS is not disable during container running the exception is thrown:

  ```
  PS D:\GitHub\kicaj29\Kubernetes\Keda\FancyMicroservice\FancyMicroservice> docker run -e ASPNETCORE_URLS="https://+:443;http://+:80" -p 4031:443 -p 4032:80 kicaj29/fancymicroservice:v1
  Unhandled exception. System.InvalidOperationException: Unable to configure HTTPS endpoint. No server certificate was specified, and the default developer certificate could not be found or is out of date.
  To generate a developer certificate run 'dotnet dev-certs https'. To trust the certificate (Windows and macOS only) run 'dotnet dev-certs https --trust'.
  For more information on configuring HTTPS see https://go.microsoft.com/fwlink/?linkid=848054.
   at Microsoft.AspNetCore.Hosting.ListenOptionsHttpsExtensions.UseHttps(ListenOptions listenOptions, Action`1 configureOptions)
  ```
* To disable HTTPS
  * Change `launchSettings.json` `"useSSL": false`
  * In `Program.cs` comment out `app.UseHttpsRedirection();`

* Build the image
`PS D:\GitHub\kicaj29\Kubernetes\Keda\FancyMicroservice\FancyMicroservice> docker build -f Dockerfile -t kicaj29/fancymicroservice:v1 ..`
More about building docker images using dockerfile created by VS can be found [here](https://learn.microsoft.com/en-us/visualstudio/containers/container-build?view=vs-2022#docker-build) and [here](https://stackoverflow.com/questions/72718492/cannot-run-docker-build-when-using-docker-setup-from-visual-studio).

* Check if the image is on the list

```
PS D:\GitHub\kicaj29\Kubernetes\Keda\FancyMicroservice\FancyMicroservice> docker images kicaj29/fancymicroservice
REPOSITORY                  TAG       IMAGE ID       CREATED          SIZE
kicaj29/fancymicroservice   v1        0f0ce5e57ce5   26 minutes ago   212MB
```

* Run the image

Set environment variable `ASPNETCORE_ENVIRONMENT` on `Development` to enable swagger page.

```
PS D:\GitHub\kicaj29\Kubernetes\Keda\FancyMicroservice\FancyMicroservice> docker run -e ASPNETCORE_ENVIRONMENT=De
velopment -p 4200:80 kicaj29/fancymicroservice:v1                                                                
info: Microsoft.Hosting.Lifetime[14]                                                                             
      Now listening on: http://[::]:80                                                                           
info: Microsoft.Hosting.Lifetime[0]                                                                              
      Application started. Press Ctrl+C to shut down.                                                            
info: Microsoft.Hosting.Lifetime[0]                                                                              
      Hosting environment: Development                                                                           
info: Microsoft.Hosting.Lifetime[0]                                                                              
      Content root path: /app/                                                                                   
```

Next test if the web api is available

http://localhost:4200/weatherforecast   
http://localhost:4200/swagger


# Create and update helm package

`helm create fancy-micro-service-helm-package`

# Verify k8s connection settings

```
kubectl config get-contexts
kubectl config use-context docker-desktop
kubectl config current-context
```
```
PS D:\GitHub\kicaj29\Kubernetes\Keda\FancyMicroservice\FancyMicroservice> kubectl get nodes
NAME             STATUS   ROLES           AGE   VERSION
docker-desktop   Ready    control-plane   64s   v1.25.4
```

>NOTE: if the K8s is not available `PS D:\GitHub\kicaj29\Kubernetes\Keda\FancyMicroservice\FancyMicroservice> kubectl get nodes
Unable to connect to the server: EOF` try to use `Reset Kubernetes Cluster` option.
![01-reset-k8s.png](./images/01-reset-k8s.png)

`helm` uses the same settings as `kubectl`.

# Verify helm package before deployment

```
PS D:\GitHub\kicaj29\Kubernetes\Keda> helm template fancy-micro-service-helm-package
PS D:\GitHub\kicaj29\Kubernetes\Keda> helm install fancy-ms fancy-micro-service-helm-package --debug --dry-run
```

If the chart already exist uninstall it:
```
PS D:\GitHub\kicaj29\Kubernetes\Keda> helm uninstall fancy-ms
release "fancy-ms" uninstalled
```

# Install fancy-ms chart

```
PS D:\GitHub\kicaj29\Kubernetes\Keda> helm install fancy-ms fancy-micro-service-helm-package
NAME: fancy-ms
LAST DEPLOYED: Wed Jul 26 15:26:47 2023
NAMESPACE: default
STATUS: deployed
REVISION: 1
NOTES:
1. Get the application URL by running these commands:
  export NODE_PORT=$(kubectl get --namespace default -o jsonpath="{.spec.ports[0].nodePort}" services fancy-ms-fancy-micro-service-helm-package)
  export NODE_IP=$(kubectl get nodes --namespace default -o jsonpath="{.items[0].status.addresses[0].address}")
  echo http://$NODE_IP:$NODE_PORT
```

# Call fancy-ms

Because by default helm chart uses `livenessProbe` and `readinessProbe` the pod fails to start.

```
PS D:\GitHub\kicaj29\Kubernetes\Keda\FancyMicroservice\FancyMicroservice> kubectl get pods
NAME                                                         READY   STATUS             RESTARTS      AGE
fancy-ms-fancy-micro-service-helm-package-76bb66974b-j2qgv   0/1     CrashLoopBackOff   5 (84s ago)   5m4s
```

```
PS D:\GitHub\kicaj29\Kubernetes\Keda\FancyMicroservice\FancyMicroservice> kubectl describe pod fancy-ms-fancy-micro-service-helm-package-76bb66974b-j2qgv
...
Events:
  Type     Reason     Age                    From               Message
  ----     ------     ----                   ----               -------
  Normal   Scheduled  4m22s                  default-scheduler  Successfully assigned default/fancy-ms-fancy-micro-service-helm-package-76bb66974b-j2qgv to docker-desktop
  Normal   Pulled     3m51s (x2 over 4m21s)  kubelet            Container image "kicaj29/fancymicroservice:v1" already present on machine
  Normal   Created    3m51s (x2 over 4m21s)  kubelet            Created container fancy-micro-service-helm-package
  Normal   Started    3m51s (x2 over 4m21s)  kubelet            Started container fancy-micro-service-helm-package
  Warning  Unhealthy  3m51s (x2 over 4m20s)  kubelet            Readiness probe failed: Get "http://10.1.0.106:80/": dial tcp 10.1.0.106:80: connect: connection refused
  Warning  Unhealthy  3m22s (x9 over 4m19s)  kubelet            Readiness probe failed: HTTP probe failed with statuscode: 404
  Warning  Unhealthy  3m22s (x6 over 4m12s)  kubelet            Liveness probe failed: HTTP probe failed with statuscode: 404
  Normal   Killing    3m22s (x2 over 3m52s)  kubelet            Container fancy-micro-service-helm-package failed liveness probe, will be restarted
```

To fix this problem necessary logic was added to the micro-service and the helm chart was updated to point correct urls.

# Install new version of the chart to fix problem with health checks

* Build new image with endpoint for health checks
```
PS D:\GitHub\kicaj29\Kubernetes\Keda\FancyMicroservice\FancyMicroservice> docker build -f Dockerfile -t kicaj29/fancymicroservice:v2 ..
```

* Uninstall previous version
```
PS D:\GitHub\kicaj29\Kubernetes\Keda> helm uninstall fancy-ms
release "fancy-ms" uninstalled
```

* Install new version
```
helm install fancy-ms fancy-micro-service-helm-package
NAME: fancy-ms
LAST DEPLOYED: Wed Jul 26 16:23:36 2023
NAMESPACE: default
STATUS: deployed
REVISION: 1
NOTES:
1. Get the application URL by running these commands:
  export NODE_PORT=$(kubectl get --namespace default -o jsonpath="{.spec.ports[0].nodePort}" services fancy-ms-fancy-micro-service-helm-package)
  export NODE_IP=$(kubectl get nodes --namespace default -o jsonpath="{.items[0].status.addresses[0].address}")
  echo http://$NODE_IP:$NODE_PORT
```

# Call fancy-ms again

Check port of the service for fancy-ms
```
PS D:\GitHub\kicaj29\Kubernetes\Keda\FancyMicroservice\FancyMicroservice> kubectl get services
NAME                                        TYPE        CLUSTER-IP      EXTERNAL-IP   PORT(S)        AGE
fancy-ms-fancy-micro-service-helm-package   NodePort    10.110.11.179   <none>        80:30317/TCP   34s
kubernetes                                  ClusterIP   10.96.0.1       <none>        443/TCP        96m
```

Next call from Chrome local machine (use localhost and not CLUSTER-IP):

```
http://localhost:30317/swagger/index.html
http://localhost:30317/weatherforecast
```