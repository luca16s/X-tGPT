FROM ubuntu:latest

RUN apt-get update

RUN apt-get install -y \
    build-essential \
    curl

RUN apt-get update

RUN apt-get install -y python3-pip

RUN pip install --upgrade pip

RUN pip install flask

RUN curl --proto '=https' --tlsv1.2 -y -sSf https://sh.rustup.rs | sh

ENV PATH="/root/.cargo/bin:${PATH}"

RUN pip install transformers

RUN pip install transformers[torch]

RUN pip install transformers[tf-cpu]

RUN pip install transformers[sentencepiece]

RUN pip install transformers[flax]

WORKDIR /app

COPY . .

CMD ["python3", "app/main.py"]