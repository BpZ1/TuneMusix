using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using TuneMusix.Data.Database.Tables.TableValues;
using TuneMusix.Helpers;
using TuneMusix.Helpers.MediaPlayer.Effects;

namespace TuneMusix.Data.Database.Tables
{
    internal class EffectsTable : TableBase<BaseEffect, EffectsTable>
    {
        public EffectsTable() : base( DatabaseTableNames.EffectsQueue ) { }

        public override void AddCommandParameter( BaseEffect effect, SQLiteCommand command )
        {
            command.Parameters.AddWithValue( Values[0].Name, effect.QueueIndex );
            command.Parameters.AddWithValue( Values[1].Name, effect.EffectType.ToString() );
            command.Parameters.AddWithValue( Values[2].Name, effect.IsActive ? 1 : 0 );
            var values = effect.GetValues();
            var numberOfValues = values.Count();
            for ( var i = 0; i < 10; i++ )
            {
                if ( i < numberOfValues )
                {
                    command.Parameters.AddWithValue( Values[i + 3].Name, values.ElementAt( i ) );
                }
                else
                {
                    command.Parameters.AddWithValue( Values[i + 3].Name, 0 );
                }
            }
        }

        public override void Delete( BaseEffect effect )
        {
            Delete( Values[0], effect.QueueIndex.ToString() );
        }

        protected override BaseEffect CreateObjectFromReader( SQLiteDataReader dbReader )
        {
            var queueIndex = dbReader.GetInt32( 0 );
            var effectType = dbReader.GetString( 1 );
            var isActive = dbReader.GetInt32( 2 ) != 0;
            var values = new List<string>();

            for ( var i = 0; i < BaseEffect.MaxValues; i++ )
            {
                var value = dbReader.GetString( i + 3 );
                values.Add( value );
            }

            var effect = GetEffectInstance( effectType );
            effect.QueueIndex = queueIndex;
            effect.IsActive = isActive;
            effect.SetValues( values );
            return effect;
        }

        private BaseEffect GetEffectInstance( string effectTypeName )
        {
            var effectType = GetEffectType( effectTypeName );
            switch ( effectType )
            {
                case EffectType.Echo:
                    return new EchoEffect();

                case EffectType.Distortion:
                    return new DistortionEffect();

                case EffectType.Compressor:
                    return new CompressorEffect();

                case EffectType.Chorus:
                    return new ChorusEffect();

                case EffectType.Equalizer:
                    return new EqualizerEffect();

                case EffectType.Flanger:
                    return new FlangerEffect();

                case EffectType.Gargle:
                    return new GargleEffect();

                case EffectType.Reverb:
                    return new ReverbEffect();

                default:
                    throw new ArgumentOutOfRangeException( $"Invalid effect type: {effectType}" );
            }
        }

        private EffectType GetEffectType( string effectTypeName )
        {
            if ( Enum.TryParse<EffectType>( effectTypeName, true, out var effect ) )
            {
                return effect;
            }
            throw new ArgumentException( $"Invalid effect type name: {effectTypeName}" );
        }

        protected override IEnumerable<SqlValue> CreateTableValues()
        {
            var values = new List<SqlValue>()
            {
                new IntSqlValue("queueIndex", true )
                {
                    IsUnique = true,
                    NotNull = true
                },
                new VarCharSqlValue("effectType", 20 )
                {
                    NotNull = true
                },
                new IntSqlValue("isActive", true),
            };
            for ( var i = 0; i < BaseEffect.MaxValues; i++ )
            {
                values.Add( new VarCharSqlValue( $"value{i}", 40 ) );
            }

            return values;
        }

        protected override IEnumerable<IDatabaseKey> GetTableKeys()
        {
            return new List<IDatabaseKey>();
        }
    }
}
