# Introduction

It allows start a pods only if a particular task is finished.


* Jobs: run to completion
  * Use cases:
    * one time initialization of resources such as databases
    * multiple workers to process messages in queue
* CronJobs: scheduled

# Commands

```
PS D:\GitHub\kicaj29\Kubernetes\ApiObjects\Job> kubectl apply -f countdown-job.yaml 
job.batch/countdown created
```

```
PS D:\GitHub\kicaj29\Kubernetes\ApiObjects\Job> kubectl get po
NAME               READY   STATUS              RESTARTS   AGE
countdown-7xn6k    0/1     ContainerCreating   0          2s
NAME               READY   STATUS      RESTARTS   AGE
countdown-7xn6k    0/1     Completed   0          32s
```

```
PS D:\GitHub\kicaj29\Kubernetes\ApiObjects\Job> kubectl logs countdown-7xn6k
9
8
7
6
5
4
3
2
1
```

Cleanup (deleting job will also delete the pod)
```
kubectl delete jobs countdown
kubectl get po
```