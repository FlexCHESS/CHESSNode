export TZ="Europe/Brussels"
export ENABLE_SBFSPOT=1
export SBFSPOT_INTERVAL=300
export ENABLE_SBFSPOT_UPLOAD=0
export DB_STORAGE="mariadb"
export CSV_STORAGE=1
export MQTT_ENABLE=1
export QUIET=0
export SBFSPOT_ARGS="-d0 -v2"
export INIT_DB=1
export MQTT_ItemFormat='"{key}": {value}'
export MQTT_ItemDelimiter=comma
export MQTT_PublisherArgs='-h {host} -t {topic} -m "{{message}}"'

start.sh &