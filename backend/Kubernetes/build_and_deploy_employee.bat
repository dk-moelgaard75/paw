cd D:\udv\paw\backend\EmployeeService\
docker build -t dkmoelgaard75/employeeservice -f "EmployeeService/Dockerfile" .
docker push dkmoelgaard75/employeeservice

cd D:\udv\paw\backend\Kubernetes\
kubectl apply -f employee-depl.yaml
kubectl rollout restart deployment employee-depl
pause