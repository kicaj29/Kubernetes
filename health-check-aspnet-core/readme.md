- [Kubernetes probes vs asp.net core healt checks](#kubernetes-probes-vs-aspnet-core-healt-checks)
- [hands-on](#hands-on)
  - [Create docker image](#create-docker-image)
  - [Helm chart](#helm-chart)
- [resources](#resources)


# Kubernetes probes vs asp.net core healt checks

- "degraded" health check maps to the "readiness" probe. The application is OK but not yet ready to serve traffic.   
For example: simple database query did succeed but took more than a second. Moving traffic to another instance is probably a good idea until the problem has resolved. It means that we should give "catch a breath" for this particular instance.   

- "unhealthy" check maps to the "liveness" probe. The application has crashed. You should shut it down and restart.

# hands-on

## Create docker image

Because Dockerfile is created by VisualStudio we have to point path to Dockerfile to be able build the image, more [here](https://stackoverflow.com/questions/66933949/failed-to-compute-cache-key-csproj-not-found).

```
/d/GitHub/kicaj29/Kubernetes/health-check-aspnet-core (master)
$ docker image build -t kicaj29/health-check-aspnet-core:1.0.0 -f health-check-aspnet-core/Dockerfile .
[+] Building 34.6s (18/18) FINISHED
 => [internal] load build definition from Dockerfile                                                                                        0.0s
 => => transferring dockerfile: 32B                                                                                                         0.0s
 => [internal] load .dockerignore                                                                                                           0.0s
 => => transferring context: 35B                                                                                                            0.0s
 => [internal] load metadata for mcr.microsoft.com/dotnet/sdk:5.0                                                                           0.3s
 => [internal] load metadata for mcr.microsoft.com/dotnet/aspnet:5.0                                                                        0.0s
 => [build 1/7] FROM mcr.microsoft.com/dotnet/sdk:5.0@sha256:d66207f54a4c1b6c8c4ff522237a00345a06d14154d9c143c6aa8b3e4f0e51bd              24.1s
 => => resolve mcr.microsoft.com/dotnet/sdk:5.0@sha256:d66207f54a4c1b6c8c4ff522237a00345a06d14154d9c143c6aa8b3e4f0e51bd                     0.0s
 => => sha256:b924cc93f8720793ed4d328fb0bcbf66c635f684d50731d04648e8fa5e9f8eee 2.01kB / 2.01kB                                              0.0s
 => => sha256:a330b6cecb98cd2425fd25fce36669073f593b3176b4ee14731e48c05d678cdd 27.15MB / 27.15MB                                            3.5s
 => => sha256:d66207f54a4c1b6c8c4ff522237a00345a06d14154d9c143c6aa8b3e4f0e51bd 2.53kB / 2.53kB                                              0.0s
 => => sha256:29a199b539f9d15b1b2c6d5eac51be280f8594431f0a4b6281eef7c13fe89ae7 7.10kB / 7.10kB                                              0.0s
 => => sha256:5116a93f798978d5b460297e7ec81278fd2b0b36baae019e36f8a8ac4811ba98 17.07MB / 17.07MB                                            2.4s
 => => sha256:9099039c5b6ed755f248b05df911c9a6d5de6a3194fe6f16719bb436a4e8d4b4 31.77MB / 31.77MB                                            6.3s
 => => sha256:3800cf5cf2534518b679158e2012be98d16be72fe681da8a2f8a59e0d95bf82d 155B / 155B                                                  2.5s
 => => sha256:98c4caca5598309368efe6070d261909bcdcf2bee5f0e709836deaa8dd578d3c 8.65MB / 8.65MB                                              3.6s
 => => sha256:7a0c5bbb8beceac9a00041e4c99870083ab7e2eae65917da26a65795d2d22487 27.56MB / 27.56MB                                            6.7s
 => => sha256:3aa88b7267aeb34d2d9bad2961b631e670691399fba932eb3c9ccd3f62240044 105.66MB / 105.66MB                                         19.3s
 => => extracting sha256:a330b6cecb98cd2425fd25fce36669073f593b3176b4ee14731e48c05d678cdd                                                   1.6s
 => => extracting sha256:5116a93f798978d5b460297e7ec81278fd2b0b36baae019e36f8a8ac4811ba98                                                   0.7s
 => => sha256:95b36d92c26324182a51a499034741ceebee23c4e0c6940f35bb0945220e5478 12.69MB / 12.69MB                                            8.2s
 => => extracting sha256:9099039c5b6ed755f248b05df911c9a6d5de6a3194fe6f16719bb436a4e8d4b4                                                   1.1s
 => => extracting sha256:3800cf5cf2534518b679158e2012be98d16be72fe681da8a2f8a59e0d95bf82d                                                   0.0s
 => => extracting sha256:98c4caca5598309368efe6070d261909bcdcf2bee5f0e709836deaa8dd578d3c                                                   0.3s
 => => extracting sha256:7a0c5bbb8beceac9a00041e4c99870083ab7e2eae65917da26a65795d2d22487                                                   1.2s
$
```

Next we can check that the image is available in the local repo:
```
$ docker images kicaj29/health-check-aspnet-core
REPOSITORY                         TAG       IMAGE ID       CREATED         SIZE
kicaj29/health-check-aspnet-core   1.0.0     eda7e2482eae   2 minutes ago   209MB
```

## Helm chart

Helm chart was created using command `helm create chart-health-check` and contains stuff like ingress which are disabled by default in
[values.yaml](./chart-health-check/values.yaml).

To test static template of the helm chart run:
```
/d/GitHub/kicaj29/Kubernetes/health-check-aspnet-core (master)
$ helm template chart-health-check
---
# Source: chart-health-check/templates/service.yaml
apiVersion: v1
kind: Service
metadata:
  name: RELEASE-NAME-chart-health-check
  labels:
    helm.sh/chart: chart-health-check-0.1.0
    app.kubernetes.io/name: chart-health-check
    app.kubernetes.io/instance: RELEASE-NAME
    app.kubernetes.io/version: "1.0.0"
    app.kubernetes.io/managed-by: Helm
spec:
  type: NodePort
  ports:
    - port: 80
      targetPort: http
      protocol: TCP
      name: http
  selector:
    app.kubernetes.io/name: chart-health-check
    app.kubernetes.io/instance: RELEASE-NAME
---
# Source: chart-health-check/templates/deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: RELEASE-NAME-chart-health-check
  labels:
    helm.sh/chart: chart-health-check-0.1.0
    app.kubernetes.io/name: chart-health-check
    app.kubernetes.io/instance: RELEASE-NAME
    app.kubernetes.io/version: "1.0.0"
    app.kubernetes.io/managed-by: Helm
spec:
  replicas: 1
  selector:
    matchLabels:
      app.kubernetes.io/name: chart-health-check
      app.kubernetes.io/instance: RELEASE-NAME
  template:
    metadata:
      labels:
        app.kubernetes.io/name: chart-health-check
        app.kubernetes.io/instance: RELEASE-NAME
    spec:
      serviceAccountName: default
      securityContext:
        {}
      containers:
        - name: chart-health-check
          securityContext:
            {}
          image: "kicaj29/health-check-aspnet-core:1.0.0"
          imagePullPolicy: IfNotPresent
          ports:
            - name: http
              containerPort: 80
              protocol: TCP
          livenessProbe:
            httpGet:
              path: /
              port: http
          readinessProbe:
            httpGet:
              path: /
              port: http
          resources:
            {}
---
# Source: chart-health-check/templates/tests/test-connection.yaml
apiVersion: v1
kind: Pod
metadata:
  name: "RELEASE-NAME-chart-health-check-test-connection"
  labels:
    helm.sh/chart: chart-health-check-0.1.0
    app.kubernetes.io/name: chart-health-check
    app.kubernetes.io/instance: RELEASE-NAME
    app.kubernetes.io/version: "1.0.0"
    app.kubernetes.io/managed-by: Helm
  annotations:
    "helm.sh/hook": test-success
spec:
  containers:
    - name: wget
      image: busybox
      command: ['wget']
      args: ['RELEASE-NAME-chart-health-check:80']
  restartPolicy: Never
```

To run dynamic test template run:

```
/d/GitHub/kicaj29/Kubernetes/health-check-aspnet-core (master)
$ helm install demo-health-checks chart-health-check --debug --dry-run
install.go:172: [debug] Original chart version: ""
install.go:189: [debug] CHART PATH: D:\GitHub\kicaj29\Kubernetes\health-check-aspnet-core\chart-health-check

NAME: demo-health-checks
LAST DEPLOYED: Fri Sep  3 16:49:48 2021
NAMESPACE: default
STATUS: pending-install
REVISION: 1
USER-SUPPLIED VALUES:
{}

COMPUTED VALUES:
affinity: {}
autoscaling:
  enabled: false
  maxReplicas: 100
  minReplicas: 1
  targetCPUUtilizationPercentage: 80
fullnameOverride: ""
image:
  pullPolicy: IfNotPresent
  repository: kicaj29/health-check-aspnet-core
  tag: 1.0.0
imagePullSecrets: []
ingress:
  annotations: {}
  enabled: false
  hosts:
  - host: chart-example.local
    paths: []
  tls: []
nameOverride: ""
nodeSelector: {}
podAnnotations: {}
podSecurityContext: {}
replicaCount: 1
resources: {}
securityContext: {}
service:
  port: 80
  type: NodePort
serviceAccount:
  annotations: {}
  create: false
  name: ""
tolerations: []

HOOKS:
---
# Source: chart-health-check/templates/tests/test-connection.yaml
apiVersion: v1
kind: Pod
metadata:
  name: "demo-health-checks-chart-health-check-test-connection"
  labels:
    helm.sh/chart: chart-health-check-0.1.0
    app.kubernetes.io/name: chart-health-check
    app.kubernetes.io/instance: demo-health-checks
    app.kubernetes.io/version: "1.0.0"
    app.kubernetes.io/managed-by: Helm
  annotations:
    "helm.sh/hook": test-success
spec:
  containers:
    - name: wget
      image: busybox
      command: ['wget']
      args: ['demo-health-checks-chart-health-check:80']
  restartPolicy: Never
MANIFEST:
---
# Source: chart-health-check/templates/service.yaml
apiVersion: v1
kind: Service
metadata:
  name: demo-health-checks-chart-health-check
  labels:
    helm.sh/chart: chart-health-check-0.1.0
    app.kubernetes.io/name: chart-health-check
    app.kubernetes.io/instance: demo-health-checks
    app.kubernetes.io/version: "1.0.0"
    app.kubernetes.io/managed-by: Helm
spec:
  type: NodePort
  ports:
    - port: 80
      targetPort: http
      protocol: TCP
      name: http
  selector:
    app.kubernetes.io/name: chart-health-check
    app.kubernetes.io/instance: demo-health-checks
---
# Source: chart-health-check/templates/deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: demo-health-checks-chart-health-check
  labels:
    helm.sh/chart: chart-health-check-0.1.0
    app.kubernetes.io/name: chart-health-check
    app.kubernetes.io/instance: demo-health-checks
    app.kubernetes.io/version: "1.0.0"
    app.kubernetes.io/managed-by: Helm
spec:
  replicas: 1
  selector:
    matchLabels:
      app.kubernetes.io/name: chart-health-check
      app.kubernetes.io/instance: demo-health-checks
  template:
    metadata:
      labels:
        app.kubernetes.io/name: chart-health-check
        app.kubernetes.io/instance: demo-health-checks
    spec:
      serviceAccountName: default
      securityContext:
        {}
      containers:
        - name: chart-health-check
          securityContext:
            {}
          image: "kicaj29/health-check-aspnet-core:1.0.0"
          imagePullPolicy: IfNotPresent
          ports:
            - name: http
              containerPort: 80
              protocol: TCP
          livenessProbe:
            httpGet:
              path: /
              port: http
          readinessProbe:
            httpGet:
              path: /
              port: http
          resources:
            {}

NOTES:
1. Get the application URL by running these commands:
  export NODE_PORT=$(kubectl get --namespace default -o jsonpath="{.spec.ports[0].nodePort}" services demo-health-checks-chart-health-check)
  export NODE_IP=$(kubectl get nodes --namespace default -o jsonpath="{.items[0].status.addresses[0].address}")
  echo http://$NODE_IP:$NODE_PORT
```

# resources

https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-5.0   
https://kubernetes.io/docs/tasks/configure-pod-container/configure-liveness-readiness-startup-probes/   