cd D:\udv\paw\backend\TaskService\
docker build -t dkmoelgaard75/taskservice -f "TaskService/Dockerfile" .
docker push dkmoelgaard75/taskservice

cd D:\udv\paw\backend\Kubernetes\
kubectl apply -f deploy-task.yaml
kubectl rollout restart deployment task-depl
pause