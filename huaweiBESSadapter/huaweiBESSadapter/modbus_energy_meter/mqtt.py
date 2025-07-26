import logging
import os
import json
import paho.mqtt.publish as mqtt_publish


def logger():
    return logging.getLogger()


def publish_data(data, topic):
    mqtt_publish.single(
        topic,
        payload=json.dumps(data),
        hostname=os.environ.get('HUAWEI_MODBUS_MQTT_BROKER'),
        auth={
            'username': os.environ.get('HUAWEI_MODBUS_MQTT_USER'),
            'password': os.environ.get('HUAWEI_MODBUS_MQTT_PASSWORD'),
        },
    )
    logger().debug(f"Published data to MQTT: {data}")
