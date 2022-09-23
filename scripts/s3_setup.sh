#!/usr/bin/env bash

# Safeties on: https://vaneyckt.io/posts/safer_bash_scripts_with_set_euxo_pipefail/
set -euo pipefail

aws --endpoint-url "$S3_ENDPOINT_URL" s3api create-bucket --bucket "${S3_BUCKET}"
aws --endpoint-url "$S3_ENDPOINT_URL" s3 cp ./scripts/tracklets.avro "s3://${S3_BUCKET}/${S3_PREFIX}"/cars.db/tracklets/
