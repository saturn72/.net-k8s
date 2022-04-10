@echo off
SERVER_CN=localhost
set OPENSSL_CONF=c:\OpenSSL-Win64\bin\openssl.cfg

echo Generate CA key:
openssl genrsa -passout pass:1111 -des3 -out ca.key 4096

echo Generate CA certificate:
openssl req -passin pass:1111 -new -x509 -days 365 -key ca.key -out ca.crt -subj  "//CN=MyRootCA"

echo Generate identity key:
openssl genrsa -passout pass:1111 -des3 -out identity.key 4096

echo Generate identity signing request:
openssl req -passin pass:1111 -new -key identity.key -out identity.csr -subj  "//CN=${SERVER_CN}"

echo Self-sign identity certificate:
openssl x509 -req -passin pass:1111 -days 365 -in identity.csr -CA ca.crt -CAkey ca.key -set_serial 01 -out identity.crt

echo Remove passphrase from identity key:
openssl rsa -passin pass:1111 -in identity.key -out identity.key

echo Generate root key:
openssl genrsa -passout pass:1111 -des3 -out root.key 4096

echo Generate root signing request:
openssl req -passin pass:1111 -new -key root.key -out root.csr -subj  "//CN=${SERVER_CN}"

echo Self-sign root certificate:
openssl x509 -req -passin pass:1111 -days 365 -in root.csr -CA ca.crt -CAkey ca.key -set_serial 01 -out root.crt

echo Remove passphrase from root key:
openssl rsa -passin pass:1111 -in root.key -out root.key


# copy to another path
# cp ca.crt ./path/to/localtion/ca.crt

# cp identity.crt ./path/to/localtion/identity.crt
# cp identity.key ./path/to/localtion/identity.key
 
# cp api.crt ./certs/api.crt
# cp api.key ./certs./path/to/localtion/api.key