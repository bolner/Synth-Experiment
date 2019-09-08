/*
    Copyright 2019 Tamas Bolner
    
    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at
    
      http://www.apache.org/licenses/LICENSE-2.0
    
    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/
using System;
using System.Threading;
using System.Globalization;

namespace Synth {
    public class Program {
        public static void Main() {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            var instrument1 = new Instrument(
                new Tuning(Tuning.Scale.Chromatic_12, 440),
                attack: 0.02, decay: 0.1, sustainLevel: 0.5, release: 0.4,
                (double time, double frequency) =>
                    2d * Waveform.Triangle(time, frequency) * (frequency / 400d) +
                    1d * Waveform.SemiSine(time, frequency) +
                    0.5d * Waveform.Square(time, frequency) * (400d / frequency) +
                    0.09d * Waveform.Noise()
            );

            var track = new Track(Track.SampleRateValue.R_44100_Hz);

            var seq = new Sequence(instrument1, "data/test.mid", filterChannel: -1, tempoChange: 1, lengthChange: 0.7);
            track.AddSequence(seq);

            track.Render(0.7, seq.TotalLength);
            track.SaveAsWavFile("data/test.wav");
        }
    }
}
