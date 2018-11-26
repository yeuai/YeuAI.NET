// Example: http://localhost:8100/api/voice/tts?text=xin%20ch%C3%A0o
function YeuAi() {
    this._context = this.__getAudioContext();
}

/**
 * Convert text to audio
 * @param {string} text
 * @param {object} options
 */
YeuAi.prototype.speak = async function (text, options) {
    const res = await fetch('http://voice.yeu.ai/api/voice/tts?text=' + text);
    const buffer = await res.arrayBuffer();
    const audioBuffer = await this._context.decodeAudioData(buffer)
    return await this.play(audioBuffer);
}

/**
 * Play audio buffer
 * @param {any} buffer
 */
YeuAi.prototype.play = function (buffer) {
    return new Promise((resolve) => {
        const source = this._context.createBufferSource();
        source.buffer = buffer;
        source.connect(this._context.destination);
        source.start();
        source.addEventListener('ended', resolve);
        // return source;
    });
}

/**
 * Get audio context
 * */
YeuAi.prototype.__getAudioContext = function () {
    let _AudioContext = AudioContext || webkitAudioContext || mozAudioContext;
    return new _AudioContext();
}

/**
 * Create global instance
 * */
window.yeuai = new YeuAi();