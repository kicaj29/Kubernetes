# List all resoruces anre their API Version
```
kubectl api-resources -o wide
```
Sample result:
```
replicasets                       rs           apps/v1                                true         ReplicaSet                       [create delete deletecollection get list patch update watch]
statefulsets                      sts          apps/v1                                true         StatefulSet                      [create delete deletecollection get list patch update watch]
tokenreviews                                   authentication.k8s.io/v1               false        TokenReview                      [create]
localsubjectaccessreviews                      authorization.k8s.io/v1                true         LocalSubjectAccessReview         [create]
selfsubjectaccessreviews                       authorization.k8s.io/v1                false        SelfSubjectAccessReview          [create]
selfsubjectrulesreviews                        authorization.k8s.io/v1                false        SelfSubjectRulesReview           [create]
subjectaccessreviews                           authorization.k8s.io/v1                false        SubjectAccessReview              [create]
horizontalpodautoscalers          hpa          autoscaling/v1                         true         HorizontalPodAutoscaler          [create delete deletecollection get list patch update watch]
cronjobs                          cj           batch/v1                               true         CronJob                          [create delete deletecollection get list patch update watch]
jobs                                           batch/v1                               true         Job                              [create delete deletecollection get list patch update watch]
```