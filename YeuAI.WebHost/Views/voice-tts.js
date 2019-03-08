/**
 * YeuAiVoice class definition
 * Example: http://localhost:9000/api/voice/tts?text=xin%20ch%C3%A0o
 */
class YeuAiVoice {
    constructor() {
        this._context = null;
    }

    /**
     * Create audio context when user clickes button `Call`
     * Ref: https://goo.gl/7K7WLu
     * Return instance of AudioContext
     */
    get context() {
        if (!this._context) {
            this._context = this.newAudioContext;
        }

        return this._context;
    }

    /**
     * Get audio context
     * */
    get newAudioContext() {
        AudioContext = AudioContext || webkitAudioContext || mozAudioContext;
        return new AudioContext();
    }

    /**
     * Convert text to audio
     * @param {string} text
     * @param {object} options
     */
    async speak(text, options) {
        const res = await fetch('https://voice.yeu.ai/api/voice/tts?text=' + text);
        const buffer = await res.arrayBuffer();
        const audioBuffer = await this.context.decodeAudioData(buffer)
        return await this.play(audioBuffer);
    }

    /**
     * Play audio buffer
     * @param {any} buffer
     */
    play(buffer) {
        return new Promise((resolve) => {
            const source = this.context.createBufferSource();
            source.buffer = buffer;
            source.connect(this.context.destination);
            source.start();
            source.addEventListener('ended', resolve);
            // return source;
        });
    }
}

/**
 * Create global instance
 * */
window.yeuaiVoice = new YeuAiVoice();
