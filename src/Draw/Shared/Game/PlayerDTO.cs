﻿using System;

namespace Draw.Shared.Game
{
    public class PlayerDTO
    {
        public PlayerDTO(string name, Guid id)
        {
            Name = name;
            Id = id;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is PlayerDTO p && this.Id.Equals(p.Id))
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
