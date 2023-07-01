import flask

from flask import request, jsonify
from transformers import pipeline

app = flask.Flask(__name__)
app.config["DEBUG"] = False


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


if __name__ == '__main__':
    app.run(host="0.0.0.0", port=5000)
