/*
 * Copyright (c) 2016 Samsung Electronics Co., Ltd All Rights Reserved
 *
 * Licensed under the Apache License, Version 2.0 (the License);
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an AS IS BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace Tizen.Multimedia
{
    /// <summary>
    /// Represents a configuration of <see cref="BarcodeDetector"/>.
    /// </summary>
    /// <seealso cref="BarcodeDetector"/>
    public class BarcodeDetectionConfiguration : EngineConfiguration
    {
        private const string KeyAttrTarget = "MV_BARCODE_DETECT_ATTR_TARGET";

        /// <summary>
        /// Initializes a new instance of the <see cref="BarcodeDetectionConfiguration"/> class.
        /// </summary>
        /// <exception cref="System.NotSupportedException">The feature is not supported.</exception>
        public BarcodeDetectionConfiguration() : base("barcode_detection")
        {
        }

        /// <summary>
        /// Gets or sets the target of the barcode detection.
        /// </summary>
        /// <exception cref="System.ArgumentException"><paramref name="value"/> is not valid.</exception>
        /// <exception cref="System.ObjectDisposedException">The <see cref="BarcodeDetectionConfiguration"/> already has been disposed of.</exception>
        public BarcodeDetectionTarget Target
        {
            get
            {
                return (BarcodeDetectionTarget)GetInt(KeyAttrTarget);
            }
            set
            {
                ValidationUtil.ValidateEnum(typeof(BarcodeDetectionTarget), value);
                Set(KeyAttrTarget, (int)value);
            }
        }
    }
}
