# Notes from course "Dynamic Jenkins Slaves with Kubernetes and Rancher"

https://www.udemy.com/course/dynamic-jenkins-slaves-with-kubernetes-and-rancher

## Node 1 - single node K8s cluster

Public IP:  54.227.63.220  
Private IP: 172.31.20.96

Steps:
* install docker (the same step like for node 1)

## Node 2 - rancher server

Public IP: 54.157.32.188  
Private IP: 172.31.18.155

Steps:
* install docker
* install rancher: https://docs.ranchermanager.rancher.io/v2.5/pages-for-subheaders/rancher-on-a-single-node-with-docker
* install RKE (Rancher Kubernetes Engine)
  * during installation we have to specify public IP of **Node 2**. Also we specify which K8s elements should be installed on Node 2 (in this case all: Control Plane, Worker Host, etcd host).
* next run `./rke_linux-amd64 up` to build the cluster.
  * it will create also file `kube_config_cluster.yml` which is used to communicate with the cluster.
* when the build is done we can go to **Node 1** and run `docker ps` to see all containers.
* install `kubectl`
* import cluster into Rancher (import existing cluster option)
  * the command must be run on this node (here is `kube_config_cluster.yml`)
  * it will create additional containers on **Node 1** for example rancher agent
  * after this in Rancher UI we should see the K8s cluster


## Jenkins

Master Dockerfile:
```
FROM jenkins/jenkins:lts 
# Plugin for scaling Jenkins agents
RUN /usr/local/bin/install-plugins.sh kubernetes
USER jenkins
```

Slave Dockerfile:
```
FROM jenkins/jnlp-slave
# Add your slave customizations here
ENTRYPOINT ["jenkins-slave"]
```

### Deploy Jenkins Master using Rancher

Import the following yml file in the Rancher UI:
```yml
apiVersion: extensions/v1beta1
kind: Deployment
metadata:
  name: jenkins
spec:
  replicas: 1
  template:
    metadata:
      labels:
        app: jenkins
    spec:
      containers:
        - name: jenkins
          image: JenkinsMaster Container!!! [Provide correct name]
          env:
            - name: JAVA_OPTS
              value: -Djenkins.install.runSetupWizard=false
          ports:
            - name: http-port
              containerPort: 8080
            - name: jnlp-port
              containerPort: 50000
          volumeMounts:
            - name: jenkins-home
              mountPath: /var/jenkins_home
      volumes:
        - name: jenkins-home
          emptyDir: {}
```
and next load balancer yml to be able open Jenkins UI:
```yml
apiVersion: v1
kind: Service
metadata:
  name: jenkins
spec:
  type: NodePort
  ports:
    - port: 8080
      targetPort: 8080
  selector:
    app: jenkins
```

### Configure Jenkins master to use Kubernetes 

https://github.com/jenkinsci/kubernetes-plugin