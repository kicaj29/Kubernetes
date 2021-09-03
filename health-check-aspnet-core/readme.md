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

# resources

https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-5.0   
https://kubernetes.io/docs/tasks/configure-pod-container/configure-liveness-readiness-startup-probes/   