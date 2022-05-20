import amqp from 'amqplib/callback_api';

const config = {
    hostname: process.env.RABBITMQ_HOSTNAME,
    port: process.env.RABBITMQ_PORT,
    username: process.env.RABBITMQ_USERNAME,
    password: process.env.RABBITMQ_PASSWORD,
}
if (!config.hostname) {
    throw new Error("Missing env varialbe: \'RABBITMQ_HOSTNAME\'");
}
if (!config.port) {
    throw new Error("Missing env varialbe: \'RABBITMQ_PORT\'");
}
if (!config.username) {
    throw new Error("Missing env varialbe: \'RABBITMQ_USERNAME\'");
}
if (!config.password) {
    throw new Error("Missing env varialbe: \'RABBITMQ_PASSWORD\'");
}
const envPrefix = process.env.RABBITMQ_ENV_PREFIX

const sendViaConnection = (exchangeName: string, routingKey: string, integrationEvent: any, closeTimeout: number = 500) => {

    try {
        amqp.connect(config, function (err0, connection) {
            if (err0) {
                throw err0;
            }
            connection.createChannel(function (err1, channel) {
                if (err1) {
                    throw err1;
                }
                channel.publish(envPrefix + exchangeName, routingKey, Buffer.from(integrationEvent));
            });
            setTimeout(function () {
                connection.close();
            }, closeTimeout);
        });
    }
    catch (err) {
        console.error(err);
    }
}

export default {
    sendToExchange: async (exchangeName: string, routingKey: string, data: any) => {
        sendViaConnection(exchangeName, routingKey, JSON.stringify({ data }));
    }
};