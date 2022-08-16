# List all resoruces and their API Version
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

# List of resources from particular API group

```
kubectl api-resources --api-group=storage.k8s.io
```
Sample result:
```
NAME                   SHORTNAMES   APIVERSION               NAMESPACED   KIND
csidrivers                          storage.k8s.io/v1        false        CSIDriver
csinodes                            storage.k8s.io/v1        false        CSINode
csistoragecapacities                storage.k8s.io/v1beta1   true         CSIStorageCapacity
storageclasses         sc           storage.k8s.io/v1        false        StorageClass
volumeattachments                   storage.k8s.io/v1        false        VolumeAttachment
```

## Next we can check which resources are actually used

```
kubectl get csidrivers,csinodes,csistoragecapacities,storageclasses,volumeattachments -A
```
Sample result:
```
NAME                                                 DRIVERS   AGE
csinode.storage.k8s.io/ip-10-2-14-189.ec2.internal   0         18d
csinode.storage.k8s.io/ip-10-2-14-88.ec2.internal    0         18d
csinode.storage.k8s.io/ip-10-2-30-153.ec2.internal   0         18d
csinode.storage.k8s.io/ip-10-2-45-38.ec2.internal    0         18d
```