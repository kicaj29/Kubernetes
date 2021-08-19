# Kubernetes probes vs asp.net core healt checks

- "degraded" health check maps to the "readiness" probe. The application is OK but not yet ready to serve traffic.
- "unhealthy" check maps to the "liveness" probe. The application has crashed. You should shut it down and restart.


# resources

https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-5.0   
https://kubernetes.io/docs/tasks/configure-pod-container/configure-liveness-readiness-startup-probes/   