﻿// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using static Retyped.dom;
using static WebHelper;

namespace Microsoft.Xna.Framework.Graphics
{
    internal partial class ConstantBuffer
    {
        private ShaderProgram _shaderProgram = null;
        private WebGLUniformLocation _location;

        static ConstantBuffer _lastConstantBufferApplied = null;

        /// <summary>
        /// A hash value which can be used to compare constant buffers.
        /// </summary>
        internal int HashKey { get; private set; }

        private void PlatformInitialize()
        {
            var data = new byte[_parameters.Length];
            for (var i = 0; i < _parameters.Length; i++)
            {
                unchecked
                {
                    data[i] = (byte)(_parameters[i] | _offsets[i]);
                }
            }

            HashKey = MonoGame.Utilities.Hash.ComputeHash(data);
        }

        private void PlatformClear()
        {
            // Force the uniform location to be looked up again
            _shaderProgram = null;
        }

        public void PlatformApply(GraphicsDevice device, ShaderProgram program)
        {
            // NOTE: We assume here the program has 
            // already been set on the device.

            // If the program changed then lookup the
            // uniform again and apply the state.
            if (_shaderProgram != program)
            {
                var location = program.GetUniformLocation(_name);
                if (location == null)
                    return;

                _shaderProgram = program;
                _location = location;
                _dirty = true;
            }

            // If the shader program is the same, the effect may still be different and have different values in the buffer
            if (!Object.ReferenceEquals(this, _lastConstantBufferApplied))
                _dirty = true;

            // If the buffer content hasn't changed then we're
            // done... use the previously set uniform state.
            if (!_dirty)
                return;

            // TODO: We need to know the type of buffer float/int/bool
            // and cast this correctly... else it doesn't work as i guess
            // GL is checking the type of the uniform.

            gl.uniform4fv(_location, _buffer.As<double[]>());
            
            GraphicsExtensions.CheckGLError();

            // Clear the dirty flag.
            _dirty = false;

            _lastConstantBufferApplied = this;
        }
    }
}
