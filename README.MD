# Kubernetes Course:

## Kubernetes Features:
  - High Availability and No Downtime
  - Scalability and High Performance
  - Disaster Recovery

## Kubernetes Components: 
  - **Pod** (Smallest unit of k8s or abstraction over container)
      - Usually 1 app per pod
      - Each pod gets its own IP address (New IP address on re-creation)
  - **Service** (Each pod have service that provide by pod but lifecycle of pod not connected to service)
      - Have permanent IP address
      - Service can share between replica nodes
      - Service have load balancer
  - **Ingress** (Service Discovery of each service of node)
  - **ConfigMap** (External configuration of application)
  - **Secret** (Used to store secret data like credentials - base64 encoded)
  - **Volumes** (Data store component of Kubernetes that can stores on local machine or remote storage)
  - **Node** (Node can have one or more (Pod+Service)) like: 
      - node1: 
            app (Pod+Service) + db (Pod+Service) + Ingress + ConfigMap + Secret + Volumes
  - **Deployment** (One or more node(s) that have only one duty to do something) - for Stateless apps
  - **StatefulSet** (One or more node(s) that have only one duty to do something) - for Stateful apps

  <img alt="Components.png" height="400" src="Resources/Components.png" width="400"/>
  <img alt="Layers.png" height="400" src="Resources/Layers.png" width="400"/>

## Kubernetes Architecture:
  - **About worker node:**
    - Each Nodes has multiple Pods on it
    - 3 Process must be installed on every node
      - Container runtime (like docker or container d)
      - kubelet (interact with both the container and node)
      - KubeProxy (Communication with services and forwards the requests)
    - Worker nodes do the actual work
  - **About Master Node:**
    - 4 Process must be installed on every node
      - Api Server (is a cluster gateway that client can communicate with kubernetes)
        - like panel or CLI
        - get request then validate then send that for process
      - Scheduler (schedule new pod or schedule terminate pod)
        - Scheduler just decides on which Node new Pod should be scheduled
        - Scheduler Know How much resource used and need, base on it decided new pod must schedule on which Node
      - Controller manager
        - detects cluster state changes (pod die, or need to re-schedule)
        - if detect one of state that must be have reaction send request to scheduler to schedule that
      - etcd (is the cluster brain, cluster changes get stored in the key-value store)
        - note: application data is not stored in etcd.

## Minikube and Kubectl:
  - **Minikube**
    - Use for test purposes.
    - Master and node processes run on one machine.
    - Have a preinstalled docker runtime as a container runtime.
    - Creates Virtual box on your machine, and nodes run on virtual box.
    - One Node k8s cluster
  - **Kubectl**
    - Enable interaction with cluster (CLI)
    - Uses for any type cluster (Minikube cluster and Cloud cluster)

## Main Kubectl Commands:
  - **CRUD Commands**
    - kubectl create deployment [name] --image=[image name]
    - kubectl edit deployment [name] --image=[image name]
    - kubectl delete deployment [name] --image=[image name]
    - kubectl apply -f [DeploymentConfig.yaml]
  - **Status of component**
    - kubectl get nodes
    - kubectl get pod
    - kubectl get services
    - kubectl get replicaset
    - kubectl get deployment
  - **Debug**
    - kubectl logs [pod name]
    - kubectl exec -it [pod name] -- bin/bash

## YAML Configuration file:
  - **Sample:**
    - Deployment & Service

```yaml 
apiVersion: apps/v1
kind: Deployment
metadata:
  name: nginx-deployment
  labels:
    app: nginx
spec:
  replicas: 3
  selector:
    matchLabels:
      app: nginx
  template:
    metadata:
      labels:
        app: nginx
    spec:
      containers:
      - name: nginx
        image: nginx:1.16
        ports:
        - containerPort: 8080
```
```yaml
apiVersion: v1
kind: Service
metadata:
  name: nginx-service
spec:
  selector:
    app: nginx
  ports:
    - protocol: TCP
      port: 80
      targetPort: 8080
```
## Sample Project
  - **Steps:**
    - Create [mongo deployment file](MongoDbSample/mongo.yaml)
    - Create [mongo secret file](MongoDbSample/mongo-secret.yaml)
    - Execute Command: `kubectl apply -f mongo-secret.yaml` in MongoDbSample directory
    - Add Secret reference to [mongo deployment file](MongoDbSample/mongo.yaml)
    - Execute Command: `kubectl apply -f mongo.yaml`
    - Add Service configuration in [mongo deployment file](MongoDbSample/mongo.yaml) after `---`
    - Create [mongo-express deployment file](MongoDbSample/mongo-express.yaml)
    - Create [mongo configmap file](MongoDbSample/mongo-configmap.yaml)
    - Execute Command: `kubectl apply -f mongo-configmap.yaml` in MongoDbSample directory
    - Add Service configuration in [mongo-express deployment file](MongoDbSample/mongo-express.yaml) after `---`
    - Execute Command: `kubectl apply -f mongo-express.yaml` in MongoDbSample directory
    - you can see status with command: `kubectl get all | grep mongo`
    - For delete deployment execute command: `kubectl delete deployment [deployment name]`
    - For delete service execute command: `kubectl delete services [service name]`

## Namespace
  - **Default namespaces**
  - Execute command: `kubectl get namespace`
    - default (if you don't create namespace your resource located here)
    - kube-public (contains public data like configmap, cluster information, ...)
    - kube-system (Don't create or modify this)
    - kube-node-lease (heartbeats of nodes, each node has lease object)
    - kubernetes-dashboard
  - you can create namespace with command: `kubectl create namespace my-namespace`
  - you can set name space of component by two approach:
    - use `--namespace=my-namespace` for example: `kubectl apply -f mongo-configmap.yaml --namespace=my-namespace`
    - use `namespace: my-namespace` in yaml file under `metadata:`

## Ingress
  - When we don't want to access to the internal service ip address and we want to use domain name and secure protocol use ingress component. for example: use `https://my-app.com` instead `http://124.8.2.1:35000`
  - Create [Mongo Ingress.yaml](MongoDbSample/Ingress.yaml)
  - Execute Command: `kubectl apply -f Ingress.yaml` in MongoDbSample directory
  - You can see the ingress list with `kubectl get ingress`
  - You can configure https and tls like [Ingress-tls.yaml](MongoDbSample/Ingress-tls.yaml) and use this [mylocalmongo-secret-tls.yaml](MongoDbSample/mylocalmongo-secret-tls.yaml)

## Helm Package
  - Helm is a package manager for kubernetes
  - Helm can help to package yaml files.
  - Helm Chart:
    - bundle of yaml files
    - Create your Own Helm Chart
    - Push them to Helm Repository
    - Download and use exist one (reusable)
  - Used for complex setups (Like monitoring app that have ELK, Grafana, Prometheus, ...)
  - You can share Helm Charts Configurations. Or use other configuration in [Helm hub](https://artifacthub.io/) or use command `helm search <Keyword>`
  - Helm has Template Engine for Same deployments.
    - for Example: Create [my-app-template.yaml](my-app-chart/Templates/my-app-template.yaml) and use `{{.Values..}}` template for read values from [values.yaml](my-app-chart/values.yaml)
  - Another feature of Helm is you can deploy same Applications to different environment.
  - Directory structure must like [my-app-chart](my-app-chart) directory.
    - [chart.yaml](my-app-chart/chart.yaml) --> meta info about chart.
    - [values.yaml](my-app-chart/values.yaml) --> values for template files.
    - [Charts folder](my-app-chart/Charts) --> chart dependencies. all yaml files that needed.
    - [Templates](my-app-chart/Templates) --> actual template files.
  - You can execute `helm install chart.yaml` to install all packages and dependencies.
  - If you have dependency you can create [requirements.yaml](my-app-chart/requirements.yaml) and execute `helm dependency update` in this section dependency added to charts folder.
  - Helm version 2 have two parts:
    - Client(Helm CLI)
    - Server(Tiller) But `Tiller` got removed in version 3 because has security issues.