rem setup pods
kubectl apply -f postgres-plat-deploy-customer.yaml
kubectl apply -f postgres-plat-deploy-employee.yaml
kubectl apply -f postgres-plat-deploy-taskobj.yaml

rem add nodeport for PgAdmin access
kubectl apply -f postgres-nodeport-customer-srv.yaml
kubectl apply -f postgres-nodeport-employee-srv.yaml
kubectl apply -f postgres-nodeport-taskobj-srv.yaml
