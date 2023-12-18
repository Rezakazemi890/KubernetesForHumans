Kuber Course:

- Kuber Features:
	- High Availability and No Downtime
	- Scalability and High Performance
	- Disaster Recovery

- Kuber Components: 
	- Pod (Smallest unit of k8s or abstraction over container)
		- Usually 1 app per pod
		- Each pod gets its own IP address (New IP address on re-creation)
	- Service (Each pod have service that provide by pod but lifecycle of pod not connected to service)
		- Have permanent IP address
		- Service can share between replica nodes
		- Service have load balancer
	- Ingress (Service Discovery of each service of node)
	- ConfigMap (External configuration of application)
	- Secret (Used to store secret data like credentials - base64 encoded)
	- Volumes (Data store component of kuber that can stores on local machin or remote storage)
	- Node (Node can have one or more (Pod+Service)) like: 
		- node1: 
			app (Pod+Service) + db (Pod+Service) + Ingress + ConfigMap + Secret + Volumes
	- Deployment (One or more node(s) that have only one duty to do something) - for Stateless apps
	- StatefulSet (One or more node(s) that have only one duty to do something) - for Stateful apps
    - ![img.png](img.png)