cp chess.yaml /tmp/chess.yaml
sed -i 's/chessmanagement/'$1'/g' /tmp/chess.yaml
sed -i 's/<CONTAINER>/'$2'/g' /tmp/chess.yaml
sed -i 's/<CREDENTIALS>/'$3'/g' /tmp/chess.yaml
sed -i 's/<ENVCONF>/'$4'/g' /tmp/chess.yaml
sed -i 's/<PORT>/'$5'/g' /tmp/chess.yaml
sed -i 's/<MOUNTNAME>/'$6'/g' /tmp/chess.yaml
sed -i 's/<MOUNTPATH>/'$7'/g' /tmp/chess.yaml
sed -i 's/<MOUNTHOSTPATH>/'$8'/g' /tmp/chess.yaml
KUBECONFIG=/app/dcconfig.txt /app/kubectl apply -f /tmp/chess.yaml 