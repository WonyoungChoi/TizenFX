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

using System;

namespace ElmSharp
{
    /// <summary>
    /// The ItemObjectExtension is used to manage item object extension
    /// </summary>
    /// <since_tizen> preview </since_tizen>
    public static class ItemObjectExtension
    {
        /// <summary>
        /// Grab high light of item object
        /// </summary>
        /// <param name="obj">the item object which is grabbed high light</param>
        /// <since_tizen> preview </since_tizen>
        public static void GrabHighlight(this ItemObject obj)
        {
            Interop.Elementary.elm_atspi_component_highlight_grab(obj.Handle);
        }

        /// <summary>
        /// Clear high light of item object
        /// </summary>
        /// <param name="obj">the item object which is cleared high light</param>
        /// <since_tizen> preview </since_tizen>
        public static void ClearHighlight(this ItemObject obj)
        {
            Interop.Elementary.elm_atspi_component_highlight_clear(obj.Handle);
        }
    }
}
