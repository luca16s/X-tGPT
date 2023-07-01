import flask
import torch
import torch.nn.functional as F

from flask import request, jsonify
from transformers import pipeline, AutoTokenizer, AutoModel

app = flask.Flask(__name__)
app.config["DEBUG"] = False


def mean_pooling(model_output, attention_mask):
    token_embeddings = model_output[0]
    input_mask_expanded = attention_mask.unsqueeze(
        -1).expand(token_embeddings.size()).float()
    return torch.sum(token_embeddings * input_mask_expanded, 1) / torch.clamp(input_mask_expanded.sum(1), min=1e-9)


@app.route('/api/v1/classify', methods=['POST'])
def classify():
    data = request.get_json()
    labels = data['labels']
    model_name = data['model']
    question = data['question']
    pipeline_name = data['pipeline']

    classifier = pipeline(pipeline_name, model=model_name)
    output = classifier(question, labels, multi_label=False)
    return jsonify(output)


@app.route('/api/v1/embeddings', methods=['POST'])
def embeddings():
    data = request.get_json()
    sentences = data['texts']
    model_name = data['model']

    tokenizer = AutoTokenizer.from_pretrained(
        model_name)
    model = AutoModel.from_pretrained(model_name)

    encoded_input = tokenizer(sentences, padding=True,
                              truncation=True, return_tensors='pt')

    with torch.no_grad():
        model_output = model(**encoded_input)

    embeddings = mean_pooling(
        model_output, encoded_input['attention_mask'])

    embeddings = F.normalize(embeddings, p=2, dim=1)

    return jsonify(embeddings.tolist())


if __name__ == '__main__':
    app.run(host="0.0.0.0", port=5000)
